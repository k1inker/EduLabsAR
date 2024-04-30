using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

namespace EduLab
{
    public class СARSpawnerNetwork : MonoBehaviourPunCallbacks
    {
        [SerializeField] private CNickNameController nickNamePlayerPrefab;
        [SerializeField] private CLevelsConfig levelsConfig;
        [SerializeField] private Transform cameraTransport;
        [SerializeField] private ARRaycastManager arRaycastManager;
        [SerializeField] private ARPlaneManager arPlaneManager;

        private int indexLevelPrefab = 0;

        //===================//
        // UNITY METHODS
        //===================//

        private void Start()
        {
            CGameMenu.Instance.OnStartGame += Init;
        }

        private void Init(int levelIndex)
        {
            StartCoroutine(this.Connect());
            this.arRaycastManager.enabled = PhotonNetwork.IsMasterClient;
            this.arPlaneManager.enabled = PhotonNetwork.IsMasterClient;

            this.indexLevelPrefab = levelIndex;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (PhotonNetwork.IsMasterClient)
            {
                EnhancedTouch.TouchSimulation.Enable();
                EnhancedTouch.EnhancedTouchSupport.Enable();
                EnhancedTouch.Touch.onFingerDown += TryPlaceLevel;
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (PhotonNetwork.IsMasterClient)
            {
                EnhancedTouch.TouchSimulation.Disable();
                EnhancedTouch.EnhancedTouchSupport.Disable();
                EnhancedTouch.Touch.onFingerDown -= TryPlaceLevel;
            }
        }

        private void OnDestroy()
        {
            CGameMenu.Instance.OnStartGame -= Init;
        }
        //===================//
        // PUBLIC METHODS
        //===================//

        //===================//
        // PRIVATE METHODS
        //===================//

        private void TryPlaceLevel(EnhancedTouch.Finger finger)
        {
            if (finger.index != 0)
            {
                return;
            }

            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (this.arRaycastManager.Raycast(finger.currentTouch.screenPosition, hits,
                    TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hits)
                {
                    GameObject levelObject = PhotonNetwork.Instantiate(this.GetPrefabLevel().name, hit.pose.position,
                        hit.pose.rotation);

                    levelObject.transform.SetParent(this.transform);

                    this.HidePlanes();
                    this.arRaycastManager.enabled = false;
                    this.enabled = false;
                    return;
                }
            }
        }

        private GameObject GetPrefabLevel()
        {
            return this.levelsConfig.GetLevelByIndex(indexLevelPrefab);
        }

        private void HidePlanes()
        {
            foreach (ARPlane arPlane in this.arPlaneManager.trackables)
            {
                arPlane.gameObject.SetActive(false);
            }

            this.arPlaneManager.enabled = false;
        }

        private IEnumerator Connect()
        {
            while (!PhotonNetwork.IsConnectedAndReady)
            {
                yield return null;
            }

            GameObject arPlayer = PhotonNetwork.Instantiate(nickNamePlayerPrefab.gameObject.name, Vector3.zero,
                Quaternion.identity);

            arPlayer.transform.SetParent(cameraTransport, false);

            this.nickNamePlayerPrefab.SetNickName(PhotonNetwork.NickName);
        }
    }
}
