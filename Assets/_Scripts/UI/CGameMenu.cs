using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EduLab
{
	public class CGameMenu : MonoBehaviourPunCallbacks
	{
		public static CGameMenu Instance { get; private set; }
		
		[Header("Menus")] 
		[SerializeField] private GameObject levelSelectMenu;
		[SerializeField] private GameObject waitingScreen;
		[SerializeField] private GameObject gameFinishMenu;
		
		[Header("GameFinishMenu")] 
		[SerializeField] private Button exitToMenu;
		
		public Action<int> OnStartGame;
		
		//===================//
		// UNITY METHODS
		//===================//
		
		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(gameObject);
			}

			this.OnStartGame += HideLevelSelectMenu;
			this.exitToMenu.onClick.AddListener(this.ExitToMenu);
		}

		private void Start()
		{
			this.waitingScreen.SetActive(!PhotonNetwork.IsMasterClient);
		}

		private void OnDestroy()
		{
			this.exitToMenu.onClick.RemoveListener(this.ExitToMenu);
			this.OnStartGame -= HideLevelSelectMenu;
		}

		//===================//
		// PUBLIC METHODS
		//===================//

		public void ShowGameFinishMenu()
		{
			this.gameFinishMenu.SetActive(true);
		}
		
		//===================//
		// PRIVATE METHODS
		//===================//

		private void HideLevelSelectMenu(int _)
		{
			this.levelSelectMenu.SetActive(false);
			this.photonView.RPC(nameof(HideWaitingScreenRPC), RpcTarget.Others);
		}
		
		[PunRPC]
		private void HideWaitingScreenRPC()
		{
			this.levelSelectMenu.SetActive(false);
			this.waitingScreen.SetActive(false);
		}

		private void ExitToMenu()
		{
			if (!PhotonNetwork.OfflineMode)
			{
				PhotonNetwork.Disconnect();
			}

			SceneManager.LoadScene("MainMenu");
		}
	}
}