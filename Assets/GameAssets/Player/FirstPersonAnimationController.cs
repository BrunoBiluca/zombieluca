using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class FirstPersonAnimationParams {
    public const string AIM = "aim";
    public const string FIRE = "fire";
    public const string RELOAD = "reload";
}

// TODO: transformar essa classe de Animation Controller em abstrata, para usar em outros projetos
// utilizar um animation controller irá facilitar na verificação de problemas com o Mechanin, 
// e a garantir desacoplamento do comportamento do objeto da animação que ele está desempenhando
public class FirstPersonAnimationController
{
    private readonly Animator animator;

    public FirstPersonAnimationController(Animator animator)
    {
        this.animator = animator;

        ValidateParams();
    }

    // TODO: transformar esse parâmetro em uma classe que é injetada na validação do projeto
    // dessa forma é posso adicionar classes de validação no container do projeto para build 
    // e remover essas validação quando for publicar o jogo.
    private void ValidateParams()
    {
        var animParams = typeof(FirstPersonAnimationParams).GetFields(
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
        )
        .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
        .Select(fi => (string)fi.GetValue(new object()))
        .ToList();

        foreach(var param in animator.parameters){
            if(!animParams.Contains(param.name))
                Debug.LogWarning($"Parameter {param.name} was not mapped on {animator.name}");
        }
    }

    public void Aim() {
        animator.SetBool(
            FirstPersonAnimationParams.AIM, 
            !animator.GetBool(FirstPersonAnimationParams.AIM)
        );
    }

    public void Fire() {
        animator.SetTrigger(FirstPersonAnimationParams.FIRE);
    }

    internal void Reload()
    {
        animator.SetTrigger(FirstPersonAnimationParams.RELOAD);
    }
}
