using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] private ParticleSystem rainParticle;
    [SerializeField] private ParticleSystem snowParticle;
    [SerializeField] private ParticleSystem thunderParticle;

    void Start()
    {
        ChangeParticle();
    }
    public void ChangeParticle()
    {
        rainParticle.Stop();
        snowParticle.Stop();
        thunderParticle.Stop();

        switch (GameManager.instance.stageTime)
        {
            case GameManager.StageTime.Rain:
                rainParticle.Play();
                break;

            case GameManager.StageTime.Snow:
                snowParticle.Play();
                break;

            case GameManager.StageTime.Thunder:
                thunderParticle.Play();
                break;
        }
    }
}