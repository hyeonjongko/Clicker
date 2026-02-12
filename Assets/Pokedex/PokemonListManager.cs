using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonListManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pokemonCardPrefab;
    public Transform contentParent;
    public Button loadMoreButton;
    public TextMeshProUGUI loadMoreButtonText;
    public GameObject loadingPanel;

    [Header("Settings")]
    public int pokemonsPerPage = 18;

    private int currentOffset = 0;
    private bool isLoading = false;

    private void Start()
    {
        LoadPokemonList();

        if (loadMoreButton != null)
        {
            loadMoreButton.onClick.AddListener(OnLoadMoreClicked);
        }
    }

    /// <summary>
    /// 포켓몬 목록을 로드합니다
    /// </summary>
    public void LoadPokemonList()
    {
        if (isLoading) return;

        StartCoroutine(LoadPokemonListCoroutine());
    }

    private IEnumerator LoadPokemonListCoroutine()
    {
        isLoading = true;

        // 로딩 표시
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        if (loadMoreButton != null)
            loadMoreButton.interactable = false;

        yield return PokeAPIManager.Instance.GetPokemonList(
            pokemonsPerPage,
            currentOffset,
            OnPokemonListLoaded,
            OnError
        );
    }

    private void OnPokemonListLoaded(PokemonListResponse response)
    {
        Debug.Log($"Loaded {response.results.Count} pokemons");

        // 각 포켓몬에 대해 카드 생성
        foreach (PokemonEntry entry in response.results)
        {
            CreatePokemonCard(entry);
        }

        // 다음 페이지를 위한 offset 업데이트
        currentOffset += pokemonsPerPage;

        // UI 업데이트
        isLoading = false;

        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        if (loadMoreButton != null)
        {
            loadMoreButton.interactable = true;
            
            // 더 로드할 포켓몬이 있는지 확인
            if (string.IsNullOrEmpty(response.next))
            {
                loadMoreButton.gameObject.SetActive(false);
            }
        }
    }

    private void CreatePokemonCard(PokemonEntry entry)
    {
        if (pokemonCardPrefab == null || contentParent == null)
        {
            Debug.LogError("Pokemon card prefab or content parent is not assigned!");
            return;
        }

        GameObject cardObj = Instantiate(pokemonCardPrefab, contentParent);
        PokemonCard card = cardObj.GetComponent<PokemonCard>();

        if (card != null)
        {
            card.SetupCard(entry);
        }
        else
        {
            Debug.LogError("PokemonCard component not found on prefab!");
        }
    }

    private void OnError(string error)
    {
        Debug.LogError($"Error loading pokemon list: {error}");
        
        isLoading = false;

        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        if (loadMoreButton != null)
            loadMoreButton.interactable = true;
    }

    private void OnLoadMoreClicked()
    {
        LoadPokemonList();
    }

    /// <summary>
    /// 목록을 초기화하고 처음부터 다시 로드합니다
    /// </summary>
    public void ResetAndReload()
    {
        // 기존 카드들 삭제
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        currentOffset = 0;
        
        if (loadMoreButton != null)
            loadMoreButton.gameObject.SetActive(true);

        LoadPokemonList();
    }
}
