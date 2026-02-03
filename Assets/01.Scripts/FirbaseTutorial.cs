using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;

public class FirbaseTutorial : MonoBehaviour
{
    private FirebaseApp _app = null;
    private FirebaseAuth _auth = null;
    private FirebaseFirestore _db = null;

    public TextMeshProUGUI _progressText;

    private void Start()
    {
        // 과제.
        // 이 씬이 시작되면
        // 1. 파이베이스 초기화
        InitFirebase(); // //Async : 비동기 함수 : 함수가 났는지 아닌지 확인할 수 없어 다음 단계로 안넘어감
        //_progressText.text = "파이어베이스 초기화 완료";
        Debug.Log("초기화 완료");
        // 2. 로그아웃
        Logout();
        Debug.Log("로그아웃 완료");
        // 3. 재로그인
        Login("hongil@skku.re.kr", "12345678");
        Debug.Log("재로그인 완료");
        // 4. 강아지 추가
        saveDogs();
        Debug.Log("강아지 추가 완료");


    }

    private void InitFirebase()
    {
        // 콜백 함수 : 특정 이벤트가 발생하고 나면 자동으로 호출되는 함수
        // 접속에 1MS ~~~ 

        //유니티는 MonoBehaviour 실행에 있어서 싱글쓰레드
        //Task 타입이란 비동기에 대한 진행사항과 완료됐을 때 결과값을 가지고 있는 객체

        // <정리 과제>
        // 1. 파이어베이스 로그인/로그아웃, CRUD를 공부
        // 1. Task, asnyc, await
        //   - 단점이 무엇인지 (메인 쓰레드로 돌아오지 않을 확률이 크다.)
        // 2. Unity만의 Task : UniTask
        //   - 장점이 무엇인지

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                // 1. 파이어베이스 연결에 성공했다면..
                _app = FirebaseApp.DefaultInstance;       // 파이어베이스 앱 모듈 가져오기
                _auth = FirebaseAuth.DefaultInstance;      // 파이어베이스 인증 모듈 가져오기 
                _db = FirebaseFirestore.DefaultInstance; // 파이어베이스  DB 모듈 가져오기

                Debug.Log("Firebase 초기화 성공!");
            }
            else
            {
                Debug.LogError("Firebase 초기화 실패: " + task.Result);
            }
        });
    }

    private void Register(string email, string password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원 가입이 취소됐습니다.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("회원가입이 실패했습니다. " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("회원가입에 성공했습니다.: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }

    private void Login(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            Firebase.Auth.AuthResult result = task.Result;

            FirebaseUser resultUser = task.Result.User;
            FirebaseUser user = _auth.CurrentUser;

            Debug.LogFormat("로그인 성공: {0} ({1})",
                result.User.Email, result.User.UserId);
        });
    }

    private void Logout()
    {
        _auth.SignOut();
        Debug.Log("로그아웃 성공");
    }

    private void CheckLoginStatus()
    {
        FirebaseUser user = _auth.CurrentUser;
        if (user == null)
        {
            Debug.Log("로그인 안됨");
        }
        else
        {
            Debug.LogFormat("로그인중: {0} ({1})",
               user.Email, user.UserId);
        }
    }

    private void saveDogs()
    {
        Dog dog = new Dog("소똥이", 4);

        //_db.Collection("Dogs").AddAsync(dog).ContinueWithOnMainThread(task =>
        _db.Collection("Dogs").Document("초코").SetAsync(dog).ContinueWithOnMainThread(task =>
        {
            // Add vs Set
            // Add : 추가한다.
            // Set : 이미 아이디의 문서가 있다면 수정하고, 없다면 추가한다.
            if (task.IsCompletedSuccessfully)
            {
                //string documentId = task.Result.Id;
                //Debug.Log("저장성공! 문서 ID : " + documentId);
                Debug.Log("저장성공!");
            }
            else
            {
                    Debug.LogError("저장 실패 :" + task.Exception);
            }
        });

    }
    private void LoadMyDog()
    {
        _db.Collection("Dogs").Document("초코").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                var snapshot = task.Result;
                if (snapshot.Exists) 
                {
                    Dog myDog = snapshot.ConvertTo<Dog>();
                    Debug.Log($"{myDog.Name}({myDog.Age})");
                }
            }
        }
        );
    }
    private void LoadDogs()
    {
        _db.Collection("Dogs").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                var snapshots = task.Result;
                Debug.Log("강아지들==============================");
                foreach (DocumentSnapshot snapshot in snapshots.Documents)
                {
                    Dog myDog = snapshot.ConvertTo<Dog>();
                    Debug.Log($"{myDog.Name})({myDog.Age})");
                }
                Debug.Log("불러오기 성공!");
            }
            else
            {
                Debug.Log("불러오기 실패" + task.Exception);
            }
        });
    }

    private void DeleteDogs()
    {
        _db.Collection("Dogs").WhereEqualTo("Name", "소똥이").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                var snapshots = task.Result;
                Debug.Log("강아지들==============================");
                foreach (DocumentSnapshot snapshot in snapshots.Documents)
                {
                    Dog myDog = snapshot.ConvertTo<Dog>();
                    Debug.Log($"{myDog.Name})({myDog.Age})");

                    if(myDog.Name == "소똥이")
                    {
                        _db.Collection("Dogs").Document(myDog.Id).DeleteAsync().ContinueWithOnMainThread(task =>
                        {
                            if (task.IsCompletedSuccessfully)
                            {
                                Debug.Log("데이터가 삭제됐습니다.");
                            }
                        });
                    }
                }
                Debug.Log("불러오기 성공!");
            }
            else
            {
                Debug.Log("불러오기 실패" + task.Exception);
            }
        });
    }

    private void Update()
    {
        if (_app == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Register("hongil@skku.re.kr", "12345678");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Login("hongil@skku.re.kr", "12345678");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Logout();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CheckLoginStatus();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            saveDogs();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            LoadMyDog();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            LoadDogs();
        }
    }
}
