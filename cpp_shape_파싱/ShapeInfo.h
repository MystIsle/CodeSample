#pragma once

#include "CoreMinimal.h"
//... 헤더 중략 ...//
#include "ShapeInfo.generated.h"

USTRUCT()
struct WHAT_THE_API FShapeInfo
{
	GENERATED_BODY()
	
	virtual ~FShapeInfo() = default;
	static TSharedPtr<FShapeInfo> CreateEmpty(EShapeType type);
	static TSharedPtr<FShapeInfo> CreateFromTable(EShapeType type, const FString& value);
	
protected:
	virtual void _initFromJson(const FString& json);
};

USTRUCT()
struct WHAT_THE_API FBoxShapeInfo : public FShapeInfo
{
	GENERATED_BODY()

	UPROPERTY()
	FVector HalfExtend;

protected:
	virtual void _initFromJson(const FString& json) override;
};

USTRUCT()
struct WHAT_THE_API FSphereShapeInfo : public FShapeInfo
{
	GENERATED_BODY()

	UPROPERTY()
	float Radius;
	
protected:
	virtual void _initFromJson(const FString& json) override;
};

/* 타입별 클래스 헤더
...
... 중략
*/

USTRUCT()
struct WHAT_THE_API FRayShapeInfo : public FShapeInfo
{
	GENERATED_BODY()

	UPROPERTY()
	float Length;
	
protected:
	virtual void _initFromJson(const FString& json) override;
};
