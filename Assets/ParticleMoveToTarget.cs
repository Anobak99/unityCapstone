using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMoveToTarget : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private GameObject boss;
    [SerializeField] private int abilityNum;
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 5f;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem.Particle[] particles;
    private int particleCount;

    private void OnEnable()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        StartCoroutine(PlayEmit());
    }

    IEnumerator PlayEmit()
    {
        particleSystem.Play();
        yield return new WaitForSeconds(2f);
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        while (true)
        {
            if (particleSystem.particleCount == 0)
            {
                break;
            }

            particles = new ParticleSystem.Particle[particleSystem.particleCount];
            particleCount = particleSystem.GetParticles(particles);

            for (int i = 0; i < particleCount; i++)
            {
                Vector3 worldPosition = transform.TransformPoint(particles[i].position); // 글로벌 위치로 변환

                if(worldPosition.x < player.position.x)
                {
                    worldPosition = Vector3.Slerp(worldPosition, new Vector3(player.position.x+0.5f, player.position.y, player.position.z),
                                                                                                                speed * Time.deltaTime);
                }
                else if (worldPosition.x == player.position.x)
                {
                    worldPosition = Vector3.Slerp(worldPosition, new Vector3(player.position.x, player.position.y-0.5f, player.position.z),
                                                                                                                speed * Time.deltaTime);
                }
                else
                {
                    worldPosition = Vector3.Slerp(worldPosition, new Vector3(player.position.x-0.5f, player.position.y, player.position.z),
                                                                                                                speed * Time.deltaTime);
                }


                particles[i].position = transform.InverseTransformPoint(worldPosition); // 다시 로컬 위치로 변환
            }

            particleSystem.SetParticles(particles, particleCount);
            yield return null;
        }

        StopEmit();
    }

    void OnParticleCollision(GameObject other)
    {
        SoundManager.PlaySound(SoundType.SFX, 1f, 9);
    }

    void StopEmit()
    {
        if (boss.activeSelf)
        {
            boss.SetActive(false);
        }
        StopAllCoroutines();
        DataManager.instance.currentData.abilities[abilityNum] = true;
        UIManager.Instance.SystemScreenON(abilityNum);
        gameObject.SetActive(false);
    }
}
