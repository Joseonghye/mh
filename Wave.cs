using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Wave : MonoBehaviour {
    
    public static Wave wave;
    public bool WaveOn = false; //웨이브 생성중인지 체크

    private int siDeCount = 0;//현재 스폰 된 공성악마 수
    private bool siDe = false; //공성악마 생성 체크
 
    private int enemyCount;//현재 스폰 된 악마 수 
    private float spawnTime = 15.0f; //스폰 간격

    public GameObject[] enemSpawner; //스포너 
    public int[] spawnerCount; //한 스포너 당 스폰된 악마 수 
    public int waveCount = 1;//현재 웨이브

    public GameObject enem_1; //악마 패턴 1
    public GameObject enem_2; 
    public GameObject crystalEnem; //공성 악마 

    public GameObject jem_f;
    public GameObject jem_i;
    public GameObject jem_s;

    public AudioSource _audio;
    public AudioClip sound_wave;


    // Use this for initialization
    void Start()
    {
        wave = this;

        enem_1.GetComponent<NavMeshAgent>().speed = 0.5f;
        enem_1.GetComponent<NavMeshAgent>().acceleration = 0.5f;

        enem_2.GetComponent<NavMeshAgent>().speed = 0.5f;
        enem_2.GetComponent<NavMeshAgent>().acceleration = 0.5f;

        crystalEnem.GetComponent<NavMeshAgent>().acceleration = 0.25f;
        
        for (int i=0; i < 10; i++)
        {
            spawnerCount[i] = 0;
        }

        //게임 3초후 시작, 15초마다 반복된다
        InvokeRepeating("RepeatWave", 3.0f, spawnTime);
        InvokeRepeating("CheckEnem", 4.0f, 0.5f);
    }

    //-----------웨이브를 반복 시키는 함수 --------------//
    void RepeatWave()
    {
        // 20마리 나오면 웨이브를 중단
        if (enemyCount == 20)
        {
            CancelInvoke("StartWave");
            CancelInvoke("CheckEnem");
            //웨이브 수 증가 
            ++waveCount;
          //  PlayerPrefs.SetInt("waveCount", waveCount);
            //악마 패턴 1 속도 증가 
            if (enem_1.GetComponent<NavMeshAgent>().acceleration < 10)
            {
                enem_1.GetComponent<NavMeshAgent>().acceleration += waveCount / 4.0f;
                enem_2.GetComponent<NavMeshAgent>().acceleration += waveCount / 4.0f;
            }
            //공성악마 속도 증가 
            if (crystalEnem.GetComponent<NavMeshAgent>().acceleration < 10)
            {
                crystalEnem.GetComponent<NavMeshAgent>().acceleration = waveCount / 4.0f;
            }

            //그 외 초기화
            siDe = false;
            WaveOn = false;
            siDeCount = 0;
            enemyCount = 0;
            for (int i = 0; i < 10; i++)
                spawnerCount[i] = 0;
        }
        _audio.PlayOneShot(sound_wave);
        InvokeRepeating("CheckEnem", 1.0f, 0.5f);
        InvokeRepeating("StartWave", 0.0f, 0.15f);
    }

    //---------------웨이브 시키는 함수----------------//
    void StartWave()
    {
        //-------------게임 끝일때---------------//
        if (GameManager.instance.GameOver)
        {
            CancelInvoke("StartWave");
            CancelInvoke("RepeatWave");
            CancelInvoke("CheckEnem");
        }
        //--------------------------------------//
        siDe = false;
        WaveOn = false;
        if (enemyCount < 20)
        {
            int rnd;
            //1마리 이하로 소환시킨 Spawner가 나올때까지 
            while (true)
            {
                rnd = Random.Range(0, 10);//0~9
                if (spawnerCount[rnd] < 2)
                {
                    //공성악마 2마리 이하일때 
                    if (siDeCount < 4 && 1 < rnd && rnd < 7) //스폰 위치3~7 사이
                    {
                        siDe = true;
                        siDeCount++;
                    }
                    WaveOn = true;
                    break;
                }
            }

            enemSpawner[rnd].GetComponent<EnemySpawner>().MakeEnemy(siDe);
            spawnerCount[rnd]++;
            enemyCount++;

        }
    }

    //-------------모든 악마가 죽었는지 체크 하는 함수--------------//
    void CheckEnem()
    {
        // 다음 웨이브 전에 모든 악마가 죽으면 2초뒤 웨이브 시작
        if (GameObject.FindGameObjectWithTag("Enemy1") == null &&
            GameObject.FindGameObjectWithTag("Enemy2") == null &&
            GameObject.FindGameObjectWithTag("CrystalEnemy") == null)
        {
            CancelInvoke("RepeatWave");
            InvokeRepeating("RepeatWave", 2.0f, 15.0f);
            CancelInvoke("CheckEnem");
        }
    }

}
