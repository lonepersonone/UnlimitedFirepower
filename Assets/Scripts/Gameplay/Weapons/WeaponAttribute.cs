using MyGame.Data.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DamageType
{
    Basics, //��ͨ�˺�
    Critical //����
}

[Serializable]
public class WeaponAttribute
{
    private GameObject weaponPrefab;
    private GameObject projectilePrefab;
    private int multiShot = 0; //�ӵ�����
    private float fireRate; //��������
    private float attenuation; //�ᴩ����
    private float damage; //�˺�
    private float range; //��Χ
    private float speed; //�ٶ�
    private float size; //��С
    private float criticalRatio; //��������
    private float criticalProbability; //��������

    // �����ٶ�ϵ���������������ߣ�
    private const float AttackSpeedCoefficient = 0.7f;

    private int multiOffset;
    private float fireRateOffset;
    private float damageOffset;
    private float speedOffset;
    private float rangeOffset;
    private float sizeOffset;
    private float attenuationOffset;
    private float criticalRatioOffset;
    private float criticalProbabilityOffset;

    public GameObject WeaponPrefab => weaponPrefab;
    public GameObject ProjectilePrefab => projectilePrefab;
    public float AttackInterval => CalculateAttackInterval(fireRateOffset);
    public float AttackSpeed => fireRate + fireRateOffset;
    public int MultiShot => multiShot + multiOffset;
    public float Damage => damage + damageOffset;
    public float Range => range + rangeOffset;
    public float Speed => speed + speedOffset;
    public float Size => size + sizeOffset;
    public float AttenuationRatio => attenuation + attenuationOffset;
    public float CriticalRatio => criticalRatio + criticalRatioOffset;
    public float CriticalProbability => criticalProbability + criticalProbabilityOffset;

    public WeaponAttribute(WeaponDataSO so)
    {
        weaponPrefab = so.WeaponPrefab;
        projectilePrefab = so.ProjectilePrefab;

        WeaponData weaponData = so.WeaponData.Clone();
        multiShot = weaponData.MultiShot;
        fireRate = weaponData.FireRate;
        damage = weaponData.Damage;
        range = weaponData.Range;
        speed = weaponData.Speed;
        size = weaponData.Size;
        attenuation = weaponData.Attenuation;
        criticalRatio = weaponData.CriticalRatio;
        criticalProbability = weaponData.CriticalProbability;
    }

    public void SetFireRateOffset(float offset){ this.fireRateOffset = offset;}
    public void SetDamageOffset(float offset) {  this.damageOffset = offset; }
    public void SetRangeOffset(float offset) { this.rangeOffset = offset; }
    public void SetSpeedOffset(float offset) { this.speedOffset = offset; }
    public void SetSizeOffset(float offset) {  this.sizeOffset = offset; }
    public void SetMultiOffset(int value) { this.multiShot = value; }
    public void SetAttenuationOffset(float offset) { this.attenuationOffset = offset; }
    public void SetCriticalRatioOffset(float offset) { this.criticalRatioOffset = offset; }
    public void SetCriticalProbabilityOffset(float offset) { this.criticalProbabilityOffset = offset; }





    // ���ݹ����ٶȰٷֱȼ���ʵ�ʹ����ٶ�
    private float CalculateAttackSpeed(float bonusAttackSpeedPercent)
    {
        // ȷ�����벻С��0
        bonusAttackSpeedPercent = Mathf.Max(0f, bonusAttackSpeedPercent);

        // ���ļ��㹫ʽ���������� �� (1 + �ӳ�ϵ�� �� ���ٰٷֱ�)
        float attackSpeed = fireRate * (1f + AttackSpeedCoefficient * bonusAttackSpeedPercent);

        // ȷ�����չ��ٲ����ڻ�������
        return Mathf.Max(fireRate, attackSpeed);
    }

    // ���㹥���������/�Σ�
    private float CalculateAttackInterval(float bonusAttackSpeedPercent)
    {
        float attackSpeed = CalculateAttackSpeed(bonusAttackSpeedPercent);
        return 1f / attackSpeed;
    }

}
