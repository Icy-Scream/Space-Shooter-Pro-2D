
using UnityEngine;

public class Ramming_Attack : MonoBehaviour
{
    [SerializeField] private float _distance;
    [SerializeField] private float _attackRange;

    private Player _player;
  
    void Start()
    {
      _player =  GameObject.FindObjectOfType<Player>();
    }
    public float RamAttack() 
    { 
       return _distance = Vector3.Distance(_player.transform.position,this.transform.position);
    
    }
}
