using UnityEngine;
using System.Collections.Generic;

public class CommandState : TurnState
{
    private AbilityMenuPanel menuPanel;

    protected int index = 0;
    protected List<string> options = new List<string>() { "Move", "Wait", "TurnEnd"};

    public override void Enter()
    {
        base.Enter();


        if (menuPanel == null)
            menuPanel = FindAnyObjectByType<AbilityMenuPanel>();
        menuPanel.Show("Action", options);
        index = 0;
        menuPanel.SetSelection(index);
    }

    public override void Exit()
    {
        base.Exit();
        menuPanel.Hide();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (e.info.y > 0)
        {
            index--;
        }
        else if (e.info.y < 0)
        {
            index++;
        }

        if (index < 0) index = options.Count - 1;
        if (index >= options.Count) index = 0;

        menuPanel.SetSelection(index);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            string choice = options[index];
            switch (choice)
            {
                case "Move":
                    owner.ChangeState<MoveToTargetState>(); 
                    Debug.Log("Move Selected");
                    break;
                case "Wait":
                    owner.ChangeState<SelectUnitState>();
                    Debug.Log("Wait Selected");
                    break;
            }
        }
        else if (e.info == 1) 
        {
            owner.ChangeState<SelectUnitState>();
            Debug.Log("Back");
        }
    }
}