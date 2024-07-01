using Godot;
using System;

public partial class Player : CharacterBody2D
{
  [Export]
  public float Speed { get; set; } = 150.0f;
  [Export]
  public float JumpForce { get; set; } = -350.0f;

  public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
  private AnimatedSprite2D anim;

  public override void _Ready()
  {
    anim = GetNode<AnimatedSprite2D>("Anim");
  }

  public override void _PhysicsProcess(double delta)
  {
    UpdateMovement(delta);
    UpdateAnimation();
  }

  private void UpdateMovement(double delta)
  {
    Vector2 velocity = Velocity;

    velocity.X = (Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left")) * Speed;

    // Aplica a gravidade
    if (!IsOnFloor())
      velocity.Y += gravity * (float)delta;

    // Mecânica de Pulo
    if (Input.IsActionJustPressed("jump") && IsOnFloor())
      velocity.Y = JumpForce;

    Velocity = velocity;
    MoveAndSlide();
  }

  private void UpdateAnimation()
  {
    //Lógica de animação ao correr
    if (Velocity.X != 0 && IsOnFloor())
    {
      anim.Play("walk");
    }
    else if (Velocity.X == 0 && IsOnFloor())
    {
      anim.Play("idle");
    }

    // Lógica de animação ao Pular
    if (Velocity.Y < 0 && !IsOnFloor())
    {
      anim.Play("jump");
    }
    else if (Velocity.Y > 0 && !IsOnFloor())
    {
      anim.Play("fall");
    }

    // Vira a direção do Sprite
    if (Velocity.X < 0)
    {
      anim.FlipH = true;
    }
    else if (Velocity.X > 0)
    {
      anim.FlipH = false;
    }
  }
}
