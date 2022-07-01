using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemy_1;
    public GameObject enemy_2;
    public GameObject CrystalEnem;

    //enemy생성 함수 
    public void MakeEnemy(bool siDe)
    {
        GameObject newEnem;

        //공성 악마가 나올 경우 
        if (siDe == true)
        {
            newEnem = Instantiate(CrystalEnem); //프리팹 생성함수
             //위치 지정
            newEnem.transform.position = transform.position;
        }
        else
        {
            //일반 악마 패턴 1,2 랜덤 
            int pathen = Random.Range(1, 3);
            if(pathen == 1)
                newEnem = Instantiate(enemy_1);
            else
                newEnem = Instantiate(enemy_2);
            //위치 지정
            newEnem.transform.position = transform.position;
        }
    }

}
