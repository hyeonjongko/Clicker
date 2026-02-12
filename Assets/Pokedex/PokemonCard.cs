using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonCard : MonoBehaviour
{
    [Header("UI References")]
    public RawImage pokemonImage;  // Image → RawImage로 변경!
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI idText;
    public GameObject loadingIndicator;
    public Button cardButton;

    private PokemonDetail pokemonDetail;

    private void Awake()
    {
        // Button이 할당되지 않았다면 자동으로 찾기
        if (cardButton == null)
        {
            cardButton = GetComponent<Button>();
        }

        // Button 클릭 이벤트 연결
        if (cardButton != null)
        {
            cardButton.onClick.AddListener(OnCardClicked);
        }
        else
        {
            Debug.LogWarning("PokemonCard: Button component not found!");
        }
    }

    /// <summary>
    /// 포켓몬 데이터로 카드를 설정합니다
    /// </summary>
    public void SetupCard(PokemonEntry entry)
    {
        // URL에서 ID 추출 (예: "https://pokeapi.co/api/v2/pokemon/1/" -> 1)
        string[] urlParts = entry.url.Split('/');
        int pokemonId = int.Parse(urlParts[urlParts.Length - 2]);

        // 이름 설정 (첫 글자 대문자)
        nameText.text = CapitalizeFirstLetter(entry.name);
        idText.text = $"#{pokemonId:000}";

        // 로딩 표시
        if (loadingIndicator != null)
            loadingIndicator.SetActive(true);

        // 상세 정보 로드
        StartCoroutine(LoadPokemonDetail(pokemonId));
    }

    private System.Collections.IEnumerator LoadPokemonDetail(int pokemonId)
    {
        yield return PokeAPIManager.Instance.GetPokemonDetailById(
            pokemonId,
            OnPokemonDetailLoaded,
            OnError
        );
    }

    private void OnPokemonDetailLoaded(PokemonDetail detail)
    {
        pokemonDetail = detail;

        // 공식 아트워크가 있으면 사용, 없으면 기본 스프라이트 사용
        string imageUrl = detail.sprites.other?.official_artwork?.front_default 
                         ?? detail.sprites.front_default;

        if (!string.IsNullOrEmpty(imageUrl))
        {
            StartCoroutine(LoadPokemonImage(imageUrl));
        }
        else
        {
            if (loadingIndicator != null)
                loadingIndicator.SetActive(false);
            Debug.LogWarning($"No image URL found for {detail.name}");
        }
    }

    private System.Collections.IEnumerator LoadPokemonImage(string imageUrl)
    {
        yield return PokeAPIManager.Instance.DownloadImage(  // DownloadSprite → DownloadImage
            imageUrl,
            OnImageLoaded,
            OnError
        );
    }

    private void OnImageLoaded(Texture2D texture)  // Sprite → Texture2D
    {
        if (pokemonImage != null)
        {
            pokemonImage.texture = texture;  // sprite → texture
        }

        if (loadingIndicator != null)
            loadingIndicator.SetActive(false);
    }

    private void OnError(string error)
    {
        Debug.LogError($"Error loading pokemon: {error}");
        
        if (loadingIndicator != null)
            loadingIndicator.SetActive(false);
    }

    private string CapitalizeFirstLetter(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        
        return char.ToUpper(text[0]) + text.Substring(1);
    }

    /// <summary>
    /// 카드 클릭 시 호출되는 메서드
    /// </summary>
    public void OnCardClicked()
    {
        if (pokemonDetail != null)
        {
            Debug.Log($"Clicked on {pokemonDetail.name} (ID: {pokemonDetail.id})");
            
            // 상세 화면 표시 (PokemonDetailPanel이 있다면)
            PokemonDetailPanel detailPanel = FindObjectOfType<PokemonDetailPanel>();
            if (detailPanel != null)
            {
                detailPanel.ShowDetail(pokemonDetail);
            }
            else
            {
                Debug.Log("PokemonDetailPanel not found. Add it to show details.");
            }
        }
        else
        {
            Debug.LogWarning("Pokemon detail not loaded yet!");
        }
    }

    /// <summary>
    /// 포켓몬 상세 정보 반환 (외부에서 접근용)
    /// </summary>
    public PokemonDetail GetPokemonDetail()
    {
        return pokemonDetail;
    }
}
