using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public List<AttackSO> Combo;
    private float _lastClickedTime;
    private float _lastComboEnd;
    private int _comboCounter;

    [SerializeField]
    private float _timeBetweenCombos = 0.2f;
    [SerializeField]
    private float _windowBetweenComboAttacks = 0.3f;

    /*[SerializeField]
    private Weapon _weaopn; */

    private Animator _animator;
    private PlayerController _playerController;

    private Vector3 _direction;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(InputManager.instance.AttackInput);
        if (InputManager.instance.AttackInput 
            && (_playerController.GetState() == PlayerController.State.MOVING 
            || _playerController.GetState() == PlayerController.State.STANDING 
            || _playerController.GetState() == PlayerController.State.ATTACKING))
        {
            Attack();
        }
        ExitAttack();
    
    }

    void Attack()
    {
        if (Time.time - _lastComboEnd > _timeBetweenCombos && _comboCounter < Combo.Count)
        {
            CancelInvoke("EndCombo");
            _playerController.SetState(PlayerController.State.ATTACKING);

            if (Time.time - _lastClickedTime >= _windowBetweenComboAttacks)
            {
                _playerController.LookAtMouse();
                // overide current attack animation
                _animator.runtimeAnimatorController = Combo[_comboCounter].AnimatorOV;
                // play new animation
                _animator.Play("Attack", 1, 0);

                // handle dmg, visual effect-----------------


                _comboCounter++;
                _lastClickedTime = Time.time;

                if (_comboCounter > Combo.Count)
                {
                    _comboCounter = 0;
                }
            }
        }
    }

    void ExitAttack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9 && _animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
        {
            Invoke("EndCombo", 0.5f);
        }
    }

    void EndCombo()
    {
        Vector3 direction = new Vector3(InputManager.instance.MoveInput.x, 0, InputManager.instance.MoveInput.y);

        if (direction == Vector3.zero) 
        { _playerController.SetState(PlayerController.State.STANDING); }
        else 
        { _playerController.SetState(PlayerController.State.MOVING); }
        
        _comboCounter = 0;
        _lastComboEnd = Time.time;
    }
}
