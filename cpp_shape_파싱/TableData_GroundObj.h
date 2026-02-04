#pragma once

#include "Actors/Collision/OverlapData.h"

class WHAT_THE_API TableData_GroundObj
	:
	public TableData_GroundObjBase,
	public IOverlapData
{
    /*코드 제네레이트 된 테이블 클래스
    ...
    ...
    ... 중략
    */

public:
	//IOverlapData
	virtual EShapeType GetShapeType() const override{ return getShapeType(); }
	virtual EEffectTargetType GetEffectTargetType() const override{ return getTargetEffectType(); }
	virtual TSharedPtr<const struct FShapeInfo> GetShapeInfo() const override { return _shapeInfo; }
	
protected:
	TSharedPtr<struct FShapeInfo> _shapeInfo;
};
