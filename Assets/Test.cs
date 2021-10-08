using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button _button;
    public Text _text;
    
    private Enemy enemy = new Enemy(100);

    private void Start()
    {
        _button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                enemy.CurrentHp.Value -= 10;
            });

        enemy.CurrentHp.SubscribeToText(_text);
        
        enemy.IsDead
            .Where(x=>x)
            .Select(x => !x)
            .SubscribeToInteractable(_button);
    }
    
    public class Enemy
    {
        public ReactiveProperty<long> CurrentHp { get; private set;}
        public IReadOnlyReactiveProperty<bool> IsDead {get;private set;}
        public Enemy(int initialHp)
        {
            // Declarative Property
            CurrentHp = new ReactiveProperty<long>(initialHp);
            IsDead = CurrentHp.Select(x => x <= 0).ToReactiveProperty();
        }
    }
}