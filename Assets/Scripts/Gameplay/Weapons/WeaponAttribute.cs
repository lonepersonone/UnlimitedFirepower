using MyGame.Data.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DamageType
{
    Basics, //普通伤害
    Critical //暴击
}

[Serializable]
public class WeaponAttribute
{
    private GameObject weaponPrefab;
    private GameObject projectilePrefab;
    private int multiShot = 0; //子弹数量
    private float fireRate; //开火速率
    private float attenuation; //贯穿属性
    private float damage; //伤害
    private float range; //范围
    private float speed; //速度
    private float size; //大小
    private float criticalRatio; //暴击倍率
    private float criticalProbability; //暴击概率

    // 攻击速度系数（控制增长曲线）
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





    // 根据攻击速度百分比计算实际攻击速度
    private float CalculateAttackSpeed(float bonusAttackSpeedPercent)
    {
        // 确保输入不小于0
        bonusAttackSpeedPercent = Mathf.Max(0f, bonusAttackSpeedPercent);

        // 核心计算公式：基础攻速 × (1 + 加成系数 × 攻速百分比)
        float attackSpeed = fireRate * (1f + AttackSpeedCoefficient * bonusAttackSpeedPercent);

        // 确保最终攻速不低于基础攻速
        return Mathf.Max(fireRate, attackSpeed);
    }

    // 计算攻击间隔（秒/次）
    private float CalculateAttackInterval(float bonusAttackSpeedPercent)
    {
        float attackSpeed = CalculateAttackSpeed(bonusAttackSpeedPercent);
        return 1f / attackSpeed;
    }

}
