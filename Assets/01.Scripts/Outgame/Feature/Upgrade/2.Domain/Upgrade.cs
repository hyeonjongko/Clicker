using System;
using UnityEngine;

//'업그레이드'라는 게임 컨텐츠의 도메인 클래스이다.
//도메인이랑 핵심 데이터와 규칙을 말한다.
//가장 먼저 만들고, 가장 나중에 바뀐다. (게임의 본질이기 떄문에)
//핵심 데이터와 규칙을 가지고 있다라는 말은 -> 응집도가 높다 -> 표현력이 높다.

public class Upgrade
{
    //기획 데이터 (기획자가 정한 값)
    // 1. 기획 테이블의 정보를 가져온다.
    public readonly UpgradeSpecData SpecData;

    // 게임 중간에 바뀌는 데이터 (플레이어가 만들어 가는 값)
    public int Level { get; private set; }

    //업그레이드 비용 : 기본 비용 + 증가량 ^ 레벨
    public Currency Cost => SpecData.BaseCost + Math.Pow(SpecData.CostMultiplier, Level);       // 지수 공식 : 기본 비용 + 증가량 ^ 레벨

    public double Damage => SpecData.BaseDamage + Level * SpecData.DamageMultiplier;          // 선형 공식 : 기본 비용 + 레벨 * 증가량

    public bool IsMaxLevel => Level >= SpecData.MaxLevel;

    // 2. 핵심 규칙(유효성)을 작성한다.
    public Upgrade(UpgradeSpecData specData, int level)
    {
        Level = level;
        SpecData = specData;

        if (specData.MaxLevel < 0) throw new System.ArgumentException($"최대 레벨은 0보다 커야합니다 : {specData.MaxLevel}");
        if (specData.BaseCost <= 0) throw new System.ArgumentException($"기본 비용은 0보다 커야합니다 : {specData.BaseCost}");
        if (specData.BaseDamage <= 0) throw new System.ArgumentException($"기본 데미지는 0보다 커야합니다 : {specData.BaseDamage}");
        if (specData.CostMultiplier <= 0) throw new System.ArgumentException($"비용 증가량은 0보다 커야합니다 : {specData.CostMultiplier}");
        if (specData.DamageMultiplier <= 0) throw new System.ArgumentException($"데미지 증가량은 0보다 커야합니다 : {specData.DamageMultiplier}");
        if (string.IsNullOrEmpty(specData.Name)) throw new System.ArgumentException("이름은 비어있을 수 없습니다.");
        if (string.IsNullOrEmpty(specData.Description)) throw new System.ArgumentException("설명은 비어있을 수 없습니다.");
    }

    public bool CanLevelUp()
    {
        return !IsMaxLevel;
    }

    public bool TryLevelUp()
    {
        if (!CanLevelUp()) return false;

        Level++;

        Debug.Log(Cost);

        return true;
    }
}
