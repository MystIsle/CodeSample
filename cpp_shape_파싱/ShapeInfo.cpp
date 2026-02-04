#include "ShapeInfo.h"
#include "JsonObjectConverter.h"

TSharedPtr<FShapeInfo> FShapeInfo::CreateEmpty(EShapeType type)
{
	FShapeInfo* result = nullptr;
	switch (type)
	{
	case EShapeType::Box:
		result = new FBoxShapeInfo();
		break;
	case EShapeType::Sphere:
		result = new FSphereShapeInfo();
		break;
    /* 타입별 생성 코드
    ...
    ... 중략
    */
   	case EShapeType::Ray:
		result = new FRayShapeInfo();
		break;
	default:
		return nullptr;
	}

	return MakeShareable(result);
}

TSharedPtr<FShapeInfo> FShapeInfo::CreateFromTable(EShapeType type, const FString& value)
{
	auto result = CreateEmpty(type);

	if (value.TrimStartAndEnd().IsEmpty() == false)
	{
		const FString jsonString = TEXT("{") + value + TEXT("}");
		result->_initFromJson(jsonString);
	}
	return result;
}

void FShapeInfo::_initFromJson(const FString& json)
{
	
}

void FBoxShapeInfo::_initFromJson(const FString& json)
{
	TSharedRef<TJsonReader<TCHAR>> reader = TJsonReaderFactory<TCHAR>::Create(json);
	TSharedPtr<FJsonObject> jsonObject = MakeShareable(new FJsonObject());
	if(FJsonSerializer::Deserialize(reader, jsonObject) == false)
	{
		return;
	}
	
	HalfExtend.X = jsonObject->GetNumberField(TEXT("X"));
	HalfExtend.Y = jsonObject->GetNumberField(TEXT("Y"));
	HalfExtend.Z = jsonObject->GetNumberField(TEXT("Z"));
}

void FSphereShapeInfo::_initFromJson(const FString& json)
{
	FJsonObjectConverter::JsonObjectStringToUStruct(json, this);
}

/* 타입별 Json 파싱 코드
...
... 중략
*/

void FRayShapeInfo::_initFromJson(const FString& json)
{
	FJsonObjectConverter::JsonObjectStringToUStruct(json, this);
}
