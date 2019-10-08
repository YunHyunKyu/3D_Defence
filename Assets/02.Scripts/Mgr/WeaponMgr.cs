using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponKinds
{
    None,
    gun,
    bow,
    sword,
    cannon,
    staff,
}

public class Weapon
{
    public int WeaponId; // id는 곧 이미지
    public string Name;
    public float FireCool;
    public float Range;
    public float Damage;
    public float Price;
    public string WeaponInfo;
    public int Grade;
    public WeaponKinds Kind;

    public Weapon()
    {
        WeaponId = -1; // -1은 인벤에 존재하지 않음을 뜻함
        Name = "";
        FireCool = 0.0f;
        Range = 0.0f;
        Damage = 0.0f;
        Price = 0.0f;
        WeaponInfo = "";
        Grade = -1;
        Kind = WeaponKinds.None;
    }

    public Weapon(int id, string name, float cool, float range, float damage, float price, int grade, WeaponKinds kind, string info)
    {
        WeaponId = id;
        Name = name;
        FireCool = cool;
        Range = range;
        Damage = damage;
        Price = price;
        Grade = grade;
        Kind = kind;
        WeaponInfo = info;
    }

    public void Init()
    {
        WeaponId = -1; // -1은 인벤에 존재하지 않음을 뜻함
        Name = "";
        FireCool = 0.0f;
        Range = 0.0f;
        Damage = 0.0f;
        Price = 0.0f;
        WeaponInfo = "";
        Grade = -1;
        Kind = WeaponKinds.None;
    }
}

//무기 종류 : 총, 활, 검, 대포, 지팡이 

public class WeaponMgr : MonoBehaviour
{
    public static List<Weapon> weaponList = new List<Weapon>();

    private void Start()
    {
        //id, name, cool, range, damage, price, grade, kind, info
        weaponList.Add(new Weapon(0, "비비탄총", 0.5f, 5f, 10f, 50f, 0, WeaponKinds.gun, "Damage:10, AttackSpeed:0.5, AttackRange:5"));
        weaponList.Add(new Weapon(1, "나무활", 1.0f, 5f, 12f, 50f, 0, WeaponKinds.bow, "Damage:12, AttackSpeed:1.0, AttackRange:5"));
        weaponList.Add(new Weapon(2, "목검", 0.3f, 10f, 15f, 50f, 0, WeaponKinds.sword, "Damage:15, AttackSpeed:0.3, AttackRange:10"));
        weaponList.Add(new Weapon(3, "장난감대포", 2.0f, 10f, 15f, 50f, 0, WeaponKinds.cannon, "Damage:15, AttackSpeed:2.0, AttackRange:10"));
        weaponList.Add(new Weapon(4, "나무지팡이", 2.0f, 15f, 20f, 50f, 0, WeaponKinds.staff, "Damage:20, AttackSpeed:2.0, AttackRange:15"));
        weaponList.Add(new Weapon(5, "총", 0.5f, 6f, 15f, 150f, 1, WeaponKinds.gun, "Damage:15, AttackSpeed:0.5, AttackRange:6"));
        weaponList.Add(new Weapon(6, "단단한활", 1.0f, 7f, 23f, 150f, 1, WeaponKinds.bow, "Damage:23, AttackSpeed:1.0, AttackRange:7"));
        weaponList.Add(new Weapon(7, "쇠검", 0.3f, 10f, 20f, 150f, 1, WeaponKinds.sword, "Damage:25, AttackSpeed:0.3, AttackRange:10"));
        weaponList.Add(new Weapon(8, "녹슨대포", 2.0f, 10f, 25f, 150f, 1, WeaponKinds.cannon, "Damage:25, AttackSpeed:2.0, AttackRange:10"));
        weaponList.Add(new Weapon(9, "단단한지팡이", 2.0f, 15f, 40f, 150f, 1, WeaponKinds.staff, "Damage:40, AttackSpeed:2.0, AttackRange:15"));
    }
}
