# 코드 샘플

## 기술 스택

- **C#** / Unity
- **C++** / Unreal Engine

---

## 샘플 목록

### 1. 뒤끝(Backend) 클라이언트 모듈 (C# / Unity)

> 뒤끝 SDK를 활용한 서버 통신 모듈

**주요 특징**
- 비동기 요청 큐 관리 (SendQueue)
- 에러 코드 파싱 및 처리
- 응답 데이터 구조화

**핵심 코드**: [`BackEndProcessor.cs`](./csharp_뒤끝_클라이언트_모듈/BackEndProcessor.cs)

---

### 2. Shape 파싱 시스템 (C++ / Unreal Engine)

> 테이블 데이터에서 충돌 Shape 정보를 파싱하는 시스템

**주요 특징**
- 팩토리 패턴을 활용한 Shape 생성
- JSON 기반 데이터 파싱
- 다형성을 활용한 타입별 처리 (Box, Sphere, Ray 등)

**핵심 코드**: [`ShapeInfo.h`](./cpp_shape_파싱/ShapeInfo.h)

---
