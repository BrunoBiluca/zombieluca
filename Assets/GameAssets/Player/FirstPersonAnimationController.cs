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
// utilizar um animation controller ir� facilitar na verifica��o de problemas com o Mechanin, 
// e a garantir desacoplamento do comportamento do objeto da anima��o que ele est� desempenhando
public class FirstPersonAnimationController
{
    private readonly Animator animator;

    public FirstPersonAnimationController(Animator animator)
    {
        this.animator = animator;

        ValidateParams();
    }

    // TODO: transformar esse par�metro em uma classe que � injetada na valida��o do projeto
    // dessa forma � posso adicionar classes de valida��o no container do projeto para build 
    // e remover essas valida��o quando for publicar o jogo.
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
