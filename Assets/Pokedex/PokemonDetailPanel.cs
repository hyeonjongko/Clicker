using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonDetailPanel : MonoBehaviour
{
    [Header("Panel Control")]
    public GameObject panelRoot;
    public Button closeButton;
    public Button backgroundButton;
    public Button prevButton;
    public Button nextButton;

    [Header("Pokemon Info")]
    public RawImage pokemonImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI idText;

    [Header("Stats")]
    public TextMeshProUGUI heightLabel;
    public TextMeshProUGUI heightValue;
    public TextMeshProUGUI weightLabel;
    public TextMeshProUGUI weightValue;
    public Transform statsContainer;
    public GameObject statBarPrefab;

    [Header("Types")]
    public Transform typesContainer;
    public GameObject typeTagPrefab;

    [Header("Description")]
    public TextMeshProUGUI descriptionText;

    [Header("Evolution")]
    public Transform evolutionContainer;
    public GameObject evolutionImagePrefab;

    [Header("Loading")]
    public GameObject loadingIndicator;

    private PokemonDetail currentPokemon;

    private void Awake()
    {
        // 버튼 이벤트 연결
        if (closeButton != null)
            closeButton.onClick.AddListener(Hide);

        if (backgroundButton != null)
            backgroundButton.onClick.AddListener(Hide);

        if (prevButton != null)
            prevButton.onClick.AddListener(OnPrevClicked);

        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextClicked);

        // 시작할 때 숨기기
        Hide();
    }

    /// <summary>
    /// 포켓몬 상세 정보를 표시합니다
    /// </summary>
    public void ShowDetail(PokemonDetail pokemon)
    {
        currentPokemon = pokemon;

        // 패널 표시
        if (panelRoot != null)
            panelRoot.SetActive(true);

        // 제목
        if (titleText != null)
            titleText.text = "Pokemon";

        // 이름 (소문자)
        if (nameText != null)
            nameText.text = pokemon.name.ToLower();

        // ID
        if (idText != null)
            idText.text = $"#{pokemon.id:D3}";

        // 키/몸무게
        if (heightLabel != null)
            heightLabel.text = "HEIGHT";
        if (heightValue != null)
            heightValue.text = $"{pokemon.height / 10f:F0} m";

        if (weightLabel != null)
            weightLabel.text = "WEIGHT";
        if (weightValue != null)
            weightValue.text = $"{pokemon.weight / 10f:F0} kg";

        // 타입 표시
        DisplayTypes(pokemon);

        // 스탯 표시
        DisplayStats(pokemon);

        // 이미지 로드
        LoadPokemonImage(pokemon);

        // 설명 로드 (별도 API 호출 필요)
        LoadPokemonSpecies(pokemon.id);

        // 네비게이션 버튼 활성화
        UpdateNavigationButtons();
    }

    private void DisplayTypes(PokemonDetail pokemon)
    {
        if (typesContainer == null) return;

        // 기존 타입 태그 제거
        foreach (Transform child in typesContainer)
        {
            Destroy(child.gameObject);
        }

        // 새 타입 태그 생성
        foreach (TypeEntry typeEntry in pokemon.types)
        {
            if (typeTagPrefab != null)
            {
                GameObject tagObj = Instantiate(typeTagPrefab, typesContainer);
                TextMeshProUGUI tagText = tagObj.GetComponentInChildren<TextMeshProUGUI>();
                
                if (tagText != null)
                {
                    tagText.text = typeEntry.type.name.ToLower();
                }

                // 타입별 색상 적용
                Image tagImage = tagObj.GetComponent<Image>();
                if (tagImage != null)
                {
                    tagImage.color = GetTypeColor(typeEntry.type.name);
                }
            }
        }
    }

    private void DisplayStats(PokemonDetail pokemon)
    {
        if (statsContainer == null) return;

        // 기존 스탯 바 제거
        foreach (Transform child in statsContainer)
        {
            Destroy(child.gameObject);
        }

        // 새 스탯 바 생성
        if (statBarPrefab != null)
        {
            foreach (StatEntry stat in pokemon.stats)
            {
                GameObject barObj = Instantiate(statBarPrefab, statsContainer);
                StatBar statBar = barObj.GetComponent<StatBar>();

                if (statBar != null)
                {
                    statBar.SetStat(stat.stat.name, stat.base_stat);
                }
            }
        }
    }

    private void LoadPokemonImage(PokemonDetail pokemon)
    {
        if (pokemonImage == null) return;

        if (loadingIndicator != null)
            loadingIndicator.SetActive(true);

        // 공식 아트워크 우선, 없으면 기본 스프라이트
        string imageUrl = pokemon.sprites.other?.official_artwork?.front_default 
                         ?? pokemon.sprites.front_default;

        if (!string.IsNullOrEmpty(imageUrl))
        {
            StartCoroutine(PokeAPIManager.Instance.DownloadImage(
                imageUrl,
                OnImageLoaded,
                OnImageError
            ));
        }
        else
        {
            if (loadingIndicator != null)
                loadingIndicator.SetActive(false);
        }
    }

    private void LoadPokemonSpecies(int pokemonId)
    {
        // 포켓몬 종(species) 정보 로드 (설명 텍스트 포함)
        StartCoroutine(PokeAPIManager.Instance.GetPokemonSpecies(
            pokemonId,
            OnSpeciesLoaded,
            OnSpeciesError
        ));
    }

    private void OnSpeciesLoaded(PokemonSpecies species)
    {
        // 영어 설명 찾기
        if (descriptionText != null && species.flavor_text_entries != null)
        {
            foreach (var entry in species.flavor_text_entries)
            {
                if (entry.language.name == "en")
                {
                    // 줄바꿈 문자 제거 및 포맷팅
                    string description = entry.flavor_text.Replace("\n", " ").Replace("\f", " ");
                    descriptionText.text = description;
                    break;
                }
            }
        }

        // 진화 체인 로드 (선택사항)
        // TODO: 진화 체인 API 구현
    }

    private void OnSpeciesError(string error)
    {
        Debug.LogWarning($"Failed to load species info: {error}");
        if (descriptionText != null)
        {
            descriptionText.text = "No description available.";
        }
    }

    private void OnImageLoaded(Texture2D texture)
    {
        if (pokemonImage != null)
        {
            pokemonImage.texture = texture;
        }

        if (loadingIndicator != null)
            loadingIndicator.SetActive(false);
    }

    private void OnImageError(string error)
    {
        Debug.LogError($"Failed to load image: {error}");
        
        if (loadingIndicator != null)
            loadingIndicator.SetActive(false);
    }

    /// <summary>
    /// 패널을 숨깁니다
    /// </summary>
    public void Hide()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);
    }

    private void OnPrevClicked()
    {
        if (currentPokemon == null) return;

        int prevId = currentPokemon.id - 1;
        if (prevId < 1) prevId = 1; // 최소 ID는 1

        LoadPokemonById(prevId);
    }

    private void OnNextClicked()
    {
        if (currentPokemon == null) return;

        int nextId = currentPokemon.id + 1;
        if (nextId > 1025) nextId = 1025; // 현재 최대 포켓몬 수

        LoadPokemonById(nextId);
    }

    private void LoadPokemonById(int pokemonId)
    {
        if (loadingIndicator != null)
            loadingIndicator.SetActive(true);

        StartCoroutine(PokeAPIManager.Instance.GetPokemonDetailById(
            pokemonId,
            ShowDetail,
            (error) => {
                Debug.LogError($"Failed to load pokemon {pokemonId}: {error}");
                if (loadingIndicator != null)
                    loadingIndicator.SetActive(false);
            }
        ));
    }

    private void UpdateNavigationButtons()
    {
        if (currentPokemon == null) return;

        // 이전 버튼 (ID가 1이면 비활성화)
        if (prevButton != null)
            prevButton.interactable = currentPokemon.id > 1;

        // 다음 버튼 (최대 ID면 비활성화)
        if (nextButton != null)
            nextButton.interactable = currentPokemon.id < 1025;
    }

    // ========== 유틸리티 함수 ==========

    private Color GetTypeColor(string typeName)
    {
        // 포켓몬 타입별 색상
        switch (typeName.ToLower())
        {
            case "normal": return new Color(0.66f, 0.66f, 0.51f);
            case "fire": return new Color(0.94f, 0.51f, 0.19f);
            case "water": return new Color(0.39f, 0.56f, 0.94f);
            case "electric": return new Color(0.98f, 0.84f, 0.19f);
            case "grass": return new Color(0.47f, 0.78f, 0.30f);
            case "ice": return new Color(0.60f, 0.85f, 0.85f);
            case "fighting": return new Color(0.75f, 0.19f, 0.16f);
            case "poison": return new Color(0.64f, 0.25f, 0.64f);
            case "ground": return new Color(0.89f, 0.76f, 0.41f);
            case "flying": return new Color(0.66f, 0.56f, 0.95f);
            case "psychic": return new Color(0.98f, 0.36f, 0.56f);
            case "bug": return new Color(0.66f, 0.76f, 0.13f);
            case "rock": return new Color(0.72f, 0.63f, 0.22f);
            case "ghost": return new Color(0.44f, 0.35f, 0.60f);
            case "dragon": return new Color(0.44f, 0.22f, 0.98f);
            case "dark": return new Color(0.44f, 0.35f, 0.27f);
            case "steel": return new Color(0.72f, 0.72f, 0.82f);
            case "fairy": return new Color(0.85f, 0.51f, 0.68f);
            default: return Color.gray;
        }
    }
}
