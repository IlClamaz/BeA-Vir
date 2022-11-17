using RenderHeads.UnityOmeka.Components;
using RenderHeads.UnityOmeka.Data;
using RenderHeads.UnityOmeka.Data.Vocabularies;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using TMPro;

namespace RenderHeads.UnityOmeka.Example
{
    /// <summary>
    /// An example script that lets us drill down from an item_set and display a piece of media
    /// </summary>
    public class DatabaseManager : MonoBehaviour
    {
        /// <summary>
        /// Reference to the API client
        /// </summary>
        [SerializeField] private OmekaClient _client = null;
        [SerializeField] private bool printDb;
        [SerializeField] private GameObject[] FindReports;
        private UnityEvent m_MyEvent;

        private void Start()
        {
            m_MyEvent = new UnityEvent();
            m_MyEvent.AddListener(SearchItemSets);
            m_MyEvent.Invoke();
        }

        /// <summary>
        /// Callback that searchs for item sets
        /// </summary>
        public async void SearchItemSets()
        {
            ItemSetSearchResponse<DublicCoreVocabulary> response = await _client.Api.SearchItemSets(new ItemSetsSearchParams()
            {
                id = 798
            });

            try
            {
                IdObject idObject = response.ItemSets[0].Id;
                m_MyEvent.RemoveAllListeners();
                m_MyEvent.AddListener(() => { SearchItemSet(idObject); });
                m_MyEvent.Invoke();
            }
            catch (System.NullReferenceException)
            {
                print("No Internet Connection!");
            }

        }


        /// <summary>
        /// Search a specific item set and returns all its items
        /// </summary>
        /// <param name="index">The ID of the item set to search</param>
        public async void SearchItemSet(IdObject index)
        {
            ItemSearchResponse<DublicCoreVocabulary> response = await _client.Api.SearchItems(new ItemSearchParams()
            {
                item_set_id = index.Id,
                per_page = 300
            });

            UpdateCanvas(response);
        }

        private void UpdateCanvas(ItemSearchResponse<DublicCoreVocabulary> response)
        {
            foreach (var item in response.Items)
            {
                foreach (var report in FindReports)
                {
                    if (int.Parse(report.name) == item.Id.Id)
                    {
                        TextMeshProUGUI[] childrenTexts = report.GetComponentsInChildren<TextMeshProUGUI>();
                        childrenTexts[1].text = item.Title;
                        if (item.Vocabulary.DCTermsDescription != null)
                            childrenTexts[2].text = item.Vocabulary.DCTermsDescription[0].Value;
                        else
                            childrenTexts[2].text = "";
                    }

                }

                if (printDb)
                {
                    if (item.Vocabulary.DCTermsDescription != null)
                        print(item.Id.Id + " " + item.Title + " " + item.Vocabulary.DCTermsDescription[0].Value);
                    else
                        print(item.Id.Id + " " + item.Title + " NO DESCRIPTION");
                }
            }
        }
    }
}
