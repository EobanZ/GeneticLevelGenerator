using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int m_currentScore;
    private bool m_isDead;
    public int CurrentScore { get { return m_currentScore; } }
    void Start()
    {
        m_currentScore = 0;
        m_isDead = false;
    }

    //TODO: Bug wegen den 2 Collidern galub ich. Pickups geben manchmal doppelte Punkte
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup")){
            m_currentScore += collision.GetComponent<Pickup>().Reward;
            GameManager.Instance.UpdateUI();
            //TODO: Pooling
            collision.gameObject.SetActive(false);
        }
    }
    /*
    public void killEnemy(IEnemy enemy)
    {   
        this.m_currentScore += enemy.getKillScore();
        GameManager.Instance.UpdateUI();

    }
    */
    //maybe add additional lives or something
     public void setDead(bool b) {
        this.m_isDead = b;
        GameManager.Instance.UpdateUI();
    }

    public void addToScore(int value)
    {
        m_currentScore += value;
    }

    public bool isDead() {
        return this.m_isDead;
    }
}
