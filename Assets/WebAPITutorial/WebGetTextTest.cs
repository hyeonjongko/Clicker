using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetTextTest : MonoBehaviour
{
    // HTTP 프로토콜을 이용해서 웹 서버에게 게이터 작업을 요청할 수 있다.
    // 작업 요청은 크게 4가지가 있다.
    // 1. 데이터 내놔     : GET
    // 2. 내 데이터 줄게  : POST
    // 3. 데이터 수정해줘 : PUT
    // 4. 데이터 삭제해줘 : DELETE

    private async void Start()
    {
        string result = await GetWebText("https://www.google.com/search?q=%EB%8B%88%ED%8C%8C+%EB%B0%94%EC%9D%B4%EB%9F%AC%EC%8A%A4&sca_esv=e5f3872605fd4577&sxsrf=ANbL-n5WOwFebNN8P73gd_ZlkSfzTcgWzw%3A1770695727079&ei=L6yKabrFBIDg2roPxfqHqAk&biw=1600&bih=865&gs_ssp=eJzj4tFP1zesLLfMrkgrKTBg9BJ-3d3xtqdH4fWGKW_mbnk9f82briUACf4R0A&oq=%EB%8B%88%ED%8C%8C+%EB%B0%94&gs_lp=Egxnd3Mtd2l6LXNlcnAiCuuLiO2MjCDrsJQqAggAMgsQLhiABBixAxiDATIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIaEC4YgAQYsQMYgwEYlwUY3AQY3gQY3wTYAQFIkVpQ2QdYnVBwBngBkAEBmAF2oAGkDKoBBDAuMTS4AQPIAQD4AQGYAg-gArwSqAIAwgIKEAAYsAMY1gQYR8ICChAuGIAEGEMYigXCAggQLhiABBixA8ICCxAAGIAEGLEDGIMBwgIZEC4YgAQYQxiKBRiXBRjcBBjeBBjfBNgBAcICCBAAGIAEGLEDwgIEEC4YA8ICGhAuGIAEGLEDGIMBGJcFGNwEGN4EGOAE2AEBwgIOEC4YgAQYsQMY0QMYxwHCAgUQABiABMICFxAuGIAEGLEDGJcFGNwEGN4EGOAE2AEBwgIREC4YgAQYsQMYgwEYxwEYrwHCAhEQLhiABBixAxjRAxiDARjHAcICExAuGAMYlwUY3AQY3gQY3wTYAQGYAwDxBXgfgBFC59UniAYBkAYKugYGCAEQARgUkgcINC4xMC43LTGgB-SFArIHBDAuMTC4B4EJwgcFMC45LjbIByyACAA&sclient=gws-wiz-serp");
        Debug.Log(result);
        //서버에게 데이터를 달라고 하는 작업은 비동기이므로 코루틴을 이용.
        StartCoroutine(GetText());
    }

    private async UniTask<string> GetWebText(string url)
    {
        var txt = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return txt;
    }

    IEnumerator GetText()
    {
        // URL이란 웹서버에게 어떤 자원(페이지/이미지/파일/데이터/API)이 있는 위치를 가리키는 주소
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com/search?q=%EB%8B%88%ED%8C%8C+%EB%B0%94%EC%9D%B4%EB%9F%AC%EC%8A%A4&sca_esv=e5f3872605fd4577&sxsrf=ANbL-n5WOwFebNN8P73gd_ZlkSfzTcgWzw%3A1770695727079&ei=L6yKabrFBIDg2roPxfqHqAk&biw=1600&bih=865&gs_ssp=eJzj4tFP1zesLLfMrkgrKTBg9BJ-3d3xtqdH4fWGKW_mbnk9f82briUACf4R0A&oq=%EB%8B%88%ED%8C%8C+%EB%B0%94&gs_lp=Egxnd3Mtd2l6LXNlcnAiCuuLiO2MjCDrsJQqAggAMgsQLhiABBixAxiDATIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIEEAAYAzIaEC4YgAQYsQMYgwEYlwUY3AQY3gQY3wTYAQFIkVpQ2QdYnVBwBngBkAEBmAF2oAGkDKoBBDAuMTS4AQPIAQD4AQGYAg-gArwSqAIAwgIKEAAYsAMY1gQYR8ICChAuGIAEGEMYigXCAggQLhiABBixA8ICCxAAGIAEGLEDGIMBwgIZEC4YgAQYQxiKBRiXBRjcBBjeBBjfBNgBAcICCBAAGIAEGLEDwgIEEC4YA8ICGhAuGIAEGLEDGIMBGJcFGNwEGN4EGOAE2AEBwgIOEC4YgAQYsQMY0QMYxwHCAgUQABiABMICFxAuGIAEGLEDGJcFGNwEGN4EGOAE2AEBwgIREC4YgAQYsQMYgwEYxwEYrwHCAhEQLhiABBixAxjRAxiDARjHAcICExAuGAMYlwUY3AQY3gQY3wTYAQGYAwDxBXgfgBFC59UniAYBkAYKugYGCAEQARgUkgcINC4xMC43LTGgB-SFArIHBDAuMTC4B4EJwgcFMC45LjbIByyACAA&sclient=gws-wiz-serp");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}
