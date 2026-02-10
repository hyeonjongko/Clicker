using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebGetImageTest : MonoBehaviour
{
    public RawImage MyImage;
    private async void Start()
    {
        //StartCoroutine(GetTexture());

        // URL이란 웹서버에게 어떤 자원(페이지/이미지/파일/데이터/API)이 있는 위치를 가리키는 주소
        // URL 구성
        // 프로토콜     : http(s)://
        // 경로(주소)   : placecats.com/400/300 (함수 이름)
        // 쿼리         : ?fit=fill&position=left (함수 매개변수)
        //              - ?로 시작하고, &로 구분한다. (?키1=값1&키2=값2&3=값3)
        //              - fit=contain
        //              - position=right
        //              ㄴ 옵션인데.. 메번 다르므로 웹서버 개발자와 이야기를 잘 하거나 문서를 잘 봐야한다.
        MyImage.texture = await GetWebTexture("https://placecats.com/400/300?fit=fill&position=left");
    }

    private async UniTask<Texture> GetWebTexture(string url)
    {
        try
        {
            var texture = ((DownloadHandlerTexture)((await UnityWebRequestTexture.GetTexture(url).SendWebRequest()).downloadHandler)).texture;
            return texture;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://wimg.sedaily.com/news/legacy/2019/07/29/1VLVSWWURE_1.jpg");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            MyImage.texture = myTexture;
        }
    }
}