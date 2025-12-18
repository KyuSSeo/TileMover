using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Unity.Cinemachine.CinemachineSplineRoll;
using Unity.VisualScripting;

//  풀 관리 컨트롤러
public class ObjectPoolController : MonoBehaviour
{
    // 키 기반으로 풀을 저장하는 딕셔너리
    private static Dictionary<string, PoolData> pools = new Dictionary<string, PoolData>();
    // 싱글톤 인스턴스
    private static ObjectPoolController instance;
    private static ObjectPoolController Instance
    {
        get
        {
            //  인스턴스 없으면 생성하기
            if (instance == null)
                CreateSharedInstance();
            return instance;
        }
    }

    private void Awake()
    {
        // 중복 인스턴스 방지
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    #region Public
    // 해당 키의 풀의 최대 개수를 재설정

    public static void SetMaxCount(string key, int maxCount)
    {
        if (!pools.ContainsKey(key))
            return;
        PoolData data = pools[key];
        data.maxCount = maxCount;
    }

    //  풀 초기화
    public static bool AddEntry(string key, GameObject prefab, int prepopulate, int maxCount)
    {
        // 이미 존재하면 실패 반환
        if (pools.ContainsKey(key))
            return false;

        PoolData data = new PoolData();
        data.prefab = prefab;
        data.maxCount = maxCount;
        data.pool = new Queue<Poolable>(prepopulate);
        pools.Add(key, data);

        // 초기 오브젝트들을 생성
        for (int i = 0; i < prepopulate; ++i)
            Enqueue(CreateInstance(key, prefab));
        //  풀 생성 성공을 반환
        return true;
    }

    //  풀 제거
    public static void ClearEntry(string key)
    {
        if (!pools.ContainsKey(key))
            return;

        PoolData data = pools[key];

        // 큐에서 꺼내며 오브젝트 제거
        while (data.pool.Count > 0)
        {
            Poolable obj = data.pool.Dequeue();
            GameObject.Destroy(obj.gameObject);
        }
        // 딕셔너리에서 제거
        pools.Remove(key);
    }

    // 풀 반환, 사용 처리
    public static void Enqueue(Poolable sender)
    {
        // null이거나 이미 풀 상태거나 풀 키가 없을 경우
        if (sender == null || sender.isPooled || !pools.ContainsKey(sender.key))
            return;

        PoolData data = pools[sender.key];
        // 최대 개수 초과 시 오브젝트 파괴
        if (data.pool.Count >= data.maxCount)
        {
            GameObject.Destroy(sender.gameObject);
            return;
        }
        // 큐에 추가하고 상태 변경
        data.pool.Enqueue(sender);
        sender.isPooled = true;
        sender.transform.SetParent(Instance.transform);
        sender.gameObject.SetActive(false);
    }

    // 오브젝트를 풀에서 꺼냄
    public static Poolable Dequeue(string key)
    {
        if (!pools.ContainsKey(key))
            return null;

        PoolData data = pools[key];

        //  풀에 사용할 오브젝트가 모자라면 생성하기
        if (data.pool.Count == 0)
            return CreateInstance(key, data.prefab);

        // 큐에서 꺼내서 반환
        Poolable obj = data.pool.Dequeue();
        obj.isPooled = false;
        return obj;
    }
    #endregion

    #region Private
    // 싱글톤 오브젝트를 생성
    private static void CreateSharedInstance()
    {
        GameObject obj = new GameObject("GameObject Pool Controller");
        DontDestroyOnLoad(obj);
        instance = obj.AddComponent<ObjectPoolController>();
    }

    private static Poolable CreateInstance(string key, GameObject prefab)
    {
        GameObject instance = Instantiate(prefab) as GameObject;
        Poolable p = instance.AddComponent<Poolable>();
        p.key = key;
        return p;
    }
    #endregion
}