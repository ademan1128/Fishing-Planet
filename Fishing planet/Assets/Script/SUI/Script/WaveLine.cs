using UnityEngine;

// このコンポーネントには必ず LineRenderer を付ける
[RequireComponent(typeof(LineRenderer))]
public class WaveLine : MonoBehaviour
{
    LineRenderer line;

    // 線を構成する頂点数（多いほど滑らか）
    public int pointCount = 100;

    // 線の横方向の長さ
    public float width = 10f;

    // 波の高さ（振幅）の最小・最大
    public float amplitudeMin = 0.05f;
    public float amplitudeMax = 0.1f;

    // 波の細かさ（周波数）の最小・最大
    public float frequencyMin = 0.5f;
    public float frequencyMax = 5f;

    // 波の流れる速さの最小・最大
    public float speedMin = 0.5f;
    public float speedMax = 5f;

    // 1つの波セグメントの長さ
    public float segmentLength = 2f;

    // 各セグメントごとの波データ
    class WaveData
    {
        public float amplitude;  // 波の高さ
        public float frequency;  // 波の細かさ
        public float speed;      // 波の移動速度
    }

    WaveData[] waves;

    void Start()
    {
        // LineRenderer取得
        line = GetComponent<LineRenderer>();

        // 頂点数を設定
        line.positionCount = pointCount;

        // width を segmentLength で分割して必要な波の数を決める
        int waveCount = Mathf.CeilToInt(width / segmentLength) + 2;

        // 波データ配列を作成
        waves = new WaveData[waveCount];

        // 各セグメントにランダムな波を生成
        for (int i = 0; i < waveCount; i++)
        {
            waves[i] = CreateWave();
        }
    }

    void Update()
    {
        // 各頂点を更新して波形を作る
        for (int i = 0; i < pointCount; i++)
        {
            // 0〜width の範囲に正規化されたx座標
            float x = (i / (float)(pointCount - 1)) * width;

            // どの波セグメントに属しているかを計算
            int waveIndex = Mathf.FloorToInt(x / segmentLength);

            // 該当セグメントの波データを取得
            WaveData wave = waves[waveIndex];

            // セグメント内でのローカル位置
            float localX = x % segmentLength;

            // sin波でyを生成（時間で流れるアニメーション）
            float y =
                Mathf.Sin((localX + Time.time * wave.speed) * wave.frequency)
                * wave.amplitude;

            // LineRenderer に座標を設定（x方向に伸びる波）
            line.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    // ランダムな波データを生成する
    WaveData CreateWave()
    {
        return new WaveData
        {
            // 波の高さをランダムに設定
            amplitude = Random.Range(amplitudeMin, amplitudeMax),

            // 波の細かさをランダムに設定
            frequency = Random.Range(frequencyMin, frequencyMax),

            // 波の速度をランダムに設定
            speed = Random.Range(speedMin, speedMax)
        };
    }
}