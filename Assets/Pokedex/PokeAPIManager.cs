using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PokeAPIManager : MonoBehaviour
{
    private const string BASE_URL = "https://pokeapi.co/api/v2";
    
    public static PokeAPIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 포켓몬 목록을 가져옵니다
    /// </summary>
    /// <param name="limit">가져올 포켓몬 개수</param>
    /// <param name="offset">시작 위치</param>
    public IEnumerator GetPokemonList(int limit, int offset, System.Action<PokemonListResponse> onSuccess, System.Action<string> onError)
    {
        string url = $"{BASE_URL}/pokemon?limit={limit}&offset={offset}";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                PokemonListResponse response = JsonUtility.FromJson<PokemonListResponse>(jsonResponse);
                onSuccess?.Invoke(response);
            }
            else
            {
                Debug.LogError($"Error: {request.error}");
                onError?.Invoke(request.error);
            }
        }
    }

    /// <summary>
    /// 특정 포켓몬의 상세 정보를 가져옵니다
    /// </summary>
    public IEnumerator GetPokemonDetail(string pokemonName, System.Action<PokemonDetail> onSuccess, System.Action<string> onError)
    {
        string url = $"{BASE_URL}/pokemon/{pokemonName.ToLower()}";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                PokemonDetail detail = JsonUtility.FromJson<PokemonDetail>(jsonResponse);
                onSuccess?.Invoke(detail);
            }
            else
            {
                Debug.LogError($"Error: {request.error}");
                onError?.Invoke(request.error);
            }
        }
    }

    /// <summary>
    /// ID로 포켓몬 상세 정보를 가져옵니다
    /// </summary>
    public IEnumerator GetPokemonDetailById(int pokemonId, System.Action<PokemonDetail> onSuccess, System.Action<string> onError)
    {
        string url = $"{BASE_URL}/pokemon/{pokemonId}";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                PokemonDetail detail = JsonUtility.FromJson<PokemonDetail>(jsonResponse);
                onSuccess?.Invoke(detail);
            }
            else
            {
                Debug.LogError($"Error: {request.error}");
                onError?.Invoke(request.error);
            }
        }
    }

    /// <summary>
    /// 포켓몬 종(species) 정보를 가져옵니다 (설명 텍스트 포함)
    /// </summary>
    public IEnumerator GetPokemonSpecies(int pokemonId, System.Action<PokemonSpecies> onSuccess, System.Action<string> onError)
    {
        string url = $"{BASE_URL}/pokemon-species/{pokemonId}";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                PokemonSpecies species = JsonUtility.FromJson<PokemonSpecies>(jsonResponse);
                onSuccess?.Invoke(species);
            }
            else
            {
                Debug.LogError($"Error: {request.error}");
                onError?.Invoke(request.error);
            }
        }
    }

    /// <summary>
    /// URL에서 이미지를 다운로드합니다 (RawImage용)
    /// </summary>
    public IEnumerator DownloadImage(string imageUrl, System.Action<Texture2D> onSuccess, System.Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                onSuccess?.Invoke(texture);
            }
            else
            {
                Debug.LogError($"Image Download Error: {request.error}");
                onError?.Invoke(request.error);
            }
        }
    }

    /// <summary>
    /// URL에서 Sprite를 다운로드합니다 (Image 컴포넌트용, 현재 미사용)
    /// 참고: RawImage를 사용하므로 DownloadImage()를 사용하세요
    /// </summary>
    public IEnumerator DownloadSprite(string imageUrl, System.Action<Sprite> onSuccess, System.Action<string> onError)
    {
        yield return DownloadImage(imageUrl, 
            (texture) => 
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                onSuccess?.Invoke(sprite);
            },
            onError);
    }
}
