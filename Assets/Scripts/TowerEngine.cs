using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerEngine : MonoBehaviour {

    public int Demage;
    public float Range;

    public float Reload;
    private float curReload;

    public int cout;
    public bool tt;

    public BaseEnemy Target;
    private Type PreffEnemy = typeof(BaseEnemy);
    public List<BaseEnemy> InRangeEnemy = new List<BaseEnemy>();



    //attacks target if it exists
    public void Attack()
    {
        
            if (Target == null)
        {
            SelectTarget();
        }
        else
        {
            Target.TakeDamage(Demage);
            if ((Target != null) && (Target.Health <= 0))
            {
                InRangeEnemy.Remove(Target);
                Target = null;
            }
        }
    }

    //select new target from list
    public void SelectTarget()
    {
        if (InRangeEnemy.Count != 0)
        {
            foreach (BaseEnemy enemy in InRangeEnemy)
            {
                if (enemy == null) InRangeEnemy.Remove(enemy); 
                if ((enemy.GetType() == PreffEnemy))
                {
                    Target = enemy;
                    break;
                }

            }
            if (Target == null)
            {
                Target = InRangeEnemy[0];
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        BaseEnemy enemy = other.gameObject.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            InRangeEnemy.Add(enemy);
            if (Target == null) Target = enemy;
            if ((enemy != Target)&&(enemy.GetType() == PreffEnemy) && (Target.GetType() != PreffEnemy)) Target = enemy;
            
      
        }


    }
    private void OnTriggerExit(Collider other)
    {
        BaseEnemy enemy = other.gameObject.GetComponent<BaseEnemy>();
       
        if (enemy != null)
        {
            if (Target == enemy)
            {
                InRangeEnemy.Remove(enemy);
                Target = null;
                SelectTarget();
            }
            InRangeEnemy.Remove(enemy);
            SelectTarget();
        }
    }


    // Use this for initialization
    void Start () {

        transform.GetComponent<CapsuleCollider>().radius = Range;

    }
	
	// Update is called once per frame
	void Update () {
        if (curReload >= Reload)
        {
            Attack();
            curReload = 0;
        }
        else curReload +=Time.deltaTime;


        if (Target != null) tt = true;
        else tt = false;

        cout = InRangeEnemy.Count;

}
}
