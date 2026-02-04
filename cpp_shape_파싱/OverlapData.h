#pragma once

#include "CoreMinimal.h"
//... 헤더 중략 ...
#include "OverlapData.generated.h"

UINTERFACE()
class UOverlapData : public UInterface
{
	GENERATED_BODY()
};

class WHAT_THE_API IOverlapData
{
	GENERATED_BODY()

public:
	virtual EEffectTargetType GetEffectTargetType() const PURE_VIRTUAL( IOverlapData::GetEffectTargetType, return EEffectTargetType::Max; );
	virtual EShapeType GetShapeType() const PURE_VIRTUAL( IOverlapData::GetEffectTargetType(), return EShapeType::Max; );
	virtual TSharedPtr<const struct FShapeInfo> GetShapeInfo() const PURE_VIRTUAL( IOverlapData::GetShapeType, return nullptr; );

	template<typename T>
	TSharedPtr<const T> GetShapeInfo() const { return StaticCastSharedPtr<const T>(GetShapeInfo()); }
};
