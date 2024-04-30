using System;
using Unity.VisualScripting;
using UnityEngine;

namespace EduLab
{
	public class CGameMenu : MonoBehaviour
	{
		public static CGameMenu Instance { get; private set; }

		[SerializeField] private GameObject levelSelectMenu;

		public Action<int> OnStartGame;
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

			this.OnStartGame += HideMenu;
		}

		private void HideMenu(int _)
		{
			this.levelSelectMenu.SetActive(false);
		}	
	}
}