using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiOperator : MonoBehaviour
{
    [SerializeField] protected GameObject AngryEmojiPrefab;
    [SerializeField] protected int EmojiCount = 10;
    [SerializeField] protected float BoundaryLength = 5.0f;
    [SerializeField] protected float MinStartHeight = 5.0f;
    [SerializeField] protected float MaxStartHeight = 10.0f;

    //------------------------------------------------------------
    //Particle系変数
    //------------------------------------------------------------



    //------------------------------------------------------------
    //TimeLine系変数
    //------------------------------------------------------------
    [SerializeField] protected float Timeline_01 = 0.5f;
    [SerializeField] protected float Timeline_02 = 3.5f;
    [SerializeField] protected float Timeline_03 = 4.0f;

    private List<GameObject> EmojiPrefabs = new List<GameObject>();
    private List<float> StartHeights = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------------------------------
        //List初期化
        //------------------------------------------------------------
        EmojiPrefabs.Clear();
        StartHeights.Clear();

        //------------------------------------------------------------
        //絵文字のオブジェクトを生成し、初期化
        //------------------------------------------------------------
        for (int i = 0; i < EmojiCount; i++)
        {
            Vector3 NewPosition = new Vector3(
                Random.Range(BoundaryLength * -1.0f, BoundaryLength),
                0.0f,
                Random.Range(BoundaryLength * -1.0f, BoundaryLength)
            );

            //Debug.Log("NewPositionX: " + NewPosition.x);
            //Debug.Log("NewPositionX: " + NewPosition.y);
            //Debug.Log("NewPositionX: " + NewPosition.z);
            //Debug.Log("------------------------------");

            GameObject EmojiPrefab = Instantiate(AngryEmojiPrefab, NewPosition, Quaternion.identity);
            EmojiPrefab.GetComponent<Transform>().SetParent(this.transform, true);
            EmojiPrefabs.Add(EmojiPrefab);
        }

        //------------------------------------------------------------
        //初期高さの設定（この高さまで、生成後、浮き上がる）
        //------------------------------------------------------------
        for (int i = 0; i < EmojiCount; i++)
        {
            float NewHeight = Random.Range(MinStartHeight, MaxStartHeight);

            StartHeights.Add(NewHeight);
        }


    }

    // Update is called once per frame
    void Update()
    {
        //------------------------------------------------------------
        //Timeline_01 初期位置まで浮き上がる
        //------------------------------------------------------------
        if (Time.time < Timeline_01)
        {
            for (int i = 0; i < EmojiPrefabs.Count; i++)
            {
                float Speed = (StartHeights[i] / Timeline_01) * Time.deltaTime;
                Vector3 Position = EmojiPrefabs[i].GetComponent<Transform>().position;
                Vector3 NewPosition = new Vector3(
                    Position.x,
                    Position.y + Speed,
                    Position.z
                );

                EmojiPrefabs[i].GetComponent<Transform>().position = NewPosition;
            }
        }
        //------------------------------------------------------------
        //Timeline_02 ゆっくり浮き上がる
        //------------------------------------------------------------
        if (Time.time < Timeline_02)
        {
            for (int i = 0; i < EmojiPrefabs.Count; i++)
            {
                float Speed = 0.1f * Time.deltaTime;
                Vector3 Position = EmojiPrefabs[i].GetComponent<Transform>().position;
                Vector3 NewPosition = new Vector3(
                    Position.x,
                    Position.y + Speed,
                    Position.z
                );

                EmojiPrefabs[i].GetComponent<Transform>().position = NewPosition;
            }
        }
        //------------------------------------------------------------
        //炎が燃え上って消える
        //------------------------------------------------------------
        if (Time.time < Timeline_03)
        {
            for (int i = 0; i < EmojiPrefabs.Count; i++)
            {
                Vector3 LocalScale = EmojiPrefabs[i].GetComponent<Transform>().localScale;

                float Speed = 0.1f * Time.deltaTime;
                Vector3 Position = EmojiPrefabs[i].GetComponent<Transform>().position;
                Vector3 NewPosition = new Vector3(
                    Position.x,
                    Position.y + Speed,
                    Position.z
                );

                EmojiPrefabs[i].GetComponent<Transform>().position = NewPosition;
            }
        }
    }

    //void GenerateParticlePrefab
}
