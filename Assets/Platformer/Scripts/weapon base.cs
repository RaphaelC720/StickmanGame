using UnityEngine;

[CreateAssetMenu(fileName = "weaponbase", menuName = "Scriptable Objects/weaponbase")]
public class weaponbase : ScriptableObject
{
    public string WeaponName;
    public float WeaponDamage;
    public float WeaponCooldown;
    public Vector2 WeaponHitBox;
}
