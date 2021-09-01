using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TestLab
{
    public class AchievementView : MonoBehaviour
    {
        private class PopUp
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }

        [SerializeField] private Image panel;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;

        private List<PopUp> _queue = new List<PopUp>();

        private bool _isShowing = false;

        private void Start()
        {
            panel.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_isShowing && _queue.Count > 0)
            {
                _isShowing = true;
                var popUp = _queue[0];
                _queue.Remove(popUp);
                StartCoroutine(ShowPanel(popUp));
            }
        }

        public void EnqueueAchievement(string title, string description)
        {
            _queue.Add(new PopUp {Title = title, Description = description});
        }

        private IEnumerator ShowPanel(PopUp popUp)
        {
            titleText.SetText(popUp.Title);
            descriptionText.SetText(popUp.Description);
            panel.gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            panel.gameObject.SetActive(false);
            _isShowing = false;
        }
    }
}