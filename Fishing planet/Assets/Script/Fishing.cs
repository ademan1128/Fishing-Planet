using UnityEngine;

public class Fishing : MonoBehaviour
{
    public GameObject Lure;
    public Transform Rodtip;
    public LineRenderer line;

    public Vector2 throwDirection = new Vector2(1f, 1f);
    //どの角度で投げるか
    public float reelSpeed = 5f;         // 巻き取りの速度
    private bool isReeling = false;      // 巻き取り中かどうかを示すフラグ

    // 放物線設定
    public int segmentCount = 20;        //放物線のセグメント数。これが多いほど滑らかになる
    public float curveHeight = 2f;       //放物線の高さ。これが大きいほど、ルアーが高く上がるようになる
    //距離しきい値
    public float parabolaThreshold = 2f; //この距離以下では放物線を描かない
    public float parabolaMaxDistance = 6f;//この距離以上では最大の放物線を描く
    public float isThrowpower = 2f;               //投げる力の大きさ

    public LineRenderer previewLine;    //投げる前の放物線のプレビュー用LineRenderer

    private Rigidbody2D LureRigidbody;  //ルアーのRigidbody2Dコンポーネントを格納する変数
    private float RestraightLine = 4f;  //直線に戻る時間
    private float Retimer = 0f;         //現在の時間を追跡する変数
    private bool isInWater = false;     //ルアーが水に入っているかどうかを示すフラグ
    private bool isMove = false;        //ルアーが動いているかどうかを示すフラグ  



    void Start()
    {
        if (Lure != null)//Lureがnullでない場合、Rigidbody2Dコンポーネントを取得
        {
            LureRigidbody = Lure.GetComponent<Rigidbody2D>();
            LureRigidbody.simulated = false;//初めは物理挙動しない
        }
        if (line != null)
        {
            line.positionCount = segmentCount; // LineRendererの頂点数を設定(竿先とルアー)
        }

    }

    // Update is called once per frame
    void Update()
    {
        Retimer += Time.deltaTime;//RestraightLineのタイマー

        if (Lure == null || Rodtip == null || LureRigidbody == null) return;
        // Lure、Rodtip、LureRigidbodyのいずれかがnullの場合は処理を中断
        //↑これできる男がやるやつ

        if (Input.GetMouseButton(0) && isReeling == false && isMove == false)
        {

            Debug.Log(isThrowpower);
            if (isThrowpower > 10f)
            {
                isThrowpower = 10f;
            }
            else
            {
                isThrowpower += 3f * Time.deltaTime;
            }
        }

        if (Input.GetMouseButtonUp(0) && isReeling == false && isMove == false)
        {

            Debug.Log(isThrowpower);
            isMove = true;
            Retimer = 0f;
            Lure.transform.position = Rodtip.position;    // ルアーを竿先の位置に移動
            LureRigidbody.linearVelocity = Vector2.zero;  // 一応、ルアーの速度をリセット。
                                                          // LinearVelocityはRigidbody2Dの速度を表すプロパティで、Vector2.zeroは(0, 0)のベクトルを意味する。これにより、ルアーが投げられる前に静止状態になる。
            LureRigidbody.simulated = true;               // ルアーの物理挙動をONにする
            LureRigidbody.AddForce(throwDirection.normalized * isThrowpower, ForceMode2D.Impulse);// ルアーに力を加える。normalizedで方向ベクトルを正規化して、isthrowpowerで力の大きさを調整。Impulseは瞬間的な力を加えるモード


        }
        if (Input.GetMouseButtonDown(1) && isInWater == true)
        {
            isReeling = true;               // 巻き取り開始
            LureRigidbody.simulated = false;// 巻き取り中は物理挙動をOFFにする
        }
        if (isReeling)
        {
            Lure.transform.position = Vector2.MoveTowards(Lure.transform.position, Rodtip.position, reelSpeed * Time.deltaTime);
            //Vector2.MoveTowards(今の位置, 目標位置, 移動距離)らしい
            // ルアーを竿先に向かって一定速度で移動

            if (Vector2.Distance(Lure.transform.position, Rodtip.position) < 0.5f)
            {
                Lure.transform.position = Rodtip.position;    // ルアーが竿先に近づいたら位置を完全に合わせる
                isReeling = false;                            // 巻き取り終了
                LureRigidbody.simulated = false;              // ルアーの物理挙動をOFFにする
                isInWater = false;                             // 水から出たとみなす
                isMove = false;
                isThrowpower = 1f;
            }
        }
        DrawDynamicLine();
        void DrawDynamicLine()
        {
            if (line == null) return;

            Vector3 start = Rodtip.position;
            Vector3 end = Lure.transform.position;

            float distance = Vector2.Distance(start, end);//ここで竿先とルアーの距離を計算

            // 巻き取り中 or 近い  この場合完全直線
            if (isReeling || distance < parabolaThreshold)
            {
                line.positionCount = 2;
                line.SetPosition(0, start);
                line.SetPosition(1, end);
                return;
            }


            float t = Mathf.InverseLerp(parabolaThreshold, parabolaMaxDistance, distance);
            //近ければ０，遠ければ１になるようにしている
            float curveT = Mathf.Clamp01(Retimer / RestraightLine);
            //Mathf.Clamp01は引数を0から1の範囲に制限する関数
            //時間を割合に変えてる


            float dynamicHeight = curveHeight * t;
            //距離に応じてたるみ量を計算。
            //parabolaThreshold以下では0、parabolaMaxDistance以上では1になるように補間

            float courvepoewr = 1f - curveT;//courveTが増えるのをここで反転させている

            line.positionCount = segmentCount;　　　　　　　　　　　  　//LineRendererの頂点数を設定

            for (int i = 0; i < segmentCount; i++)　　　　　　　　　    //一つずつ位置を決めている
            {
                float tPos = i / (float)(segmentCount - 1);　　　　　 　//tPosは今何割目かを表している

                Vector3 point = Vector3.Lerp(start, end, tPos);　　　 　//startからendに向かって、どれくらい進んでいるかを表している

                float height = dynamicHeight * (tPos * (1 - tPos)) * 4f * courvepoewr;//真ん中を最大の高さにする計算
                //dynamicHeight(たるみの強さ)* (tPos * (1 - tPos))(これが0から1の間で、真ん中で最大になる関数) * 4f(これも強さ調整) * courvepoewr(時間経過に応じてたるみが減るようにする)
                point.y += height;　　　　　　　　　　　　　　　　　　　//ここで放物線の高さ足す

                line.SetPosition(i, point);
            }
        }
    }
        void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sea"))
        {
            isInWater = true;
            LureRigidbody.linearDamping = 5f;  // 海に入ったらルアーの動きを減速させる
            LureRigidbody.angularDamping = 5f; // 海に入ったらルアーの回転も減速させる
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Sea"))
        {
            LureRigidbody.linearDamping = 0f;  // 海から出たらルアーの動きを元に戻す
            LureRigidbody.angularDamping = 0f; // 海から出たらルアーの回転も元に戻す
        }
    }
}
