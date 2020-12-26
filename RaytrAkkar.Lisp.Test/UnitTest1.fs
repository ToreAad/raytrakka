module RaytrAkkar.Lisp.Test

open NUnit.Framework
open Parser
open LispTypes
open Evaluator
open ResultMonad

[<SetUp>]
let Setup () =
    ()

[<Test>]
let TestPlusParse () =
    let exp = "(+ 1.0 1.0)"
    let expected = LispList [LispAtom "+"; LispNumber 1.0; LispNumber 1.0]
    let actual  = match parser exp with
                | Microsoft.FSharp.Core.Ok h -> h
    Assert.AreEqual (expected.ToString(), actual.ToString())

[<Test>]
let TestPlusEval() =
    let exp = "(+ 1.0 1.0)"
    let expected = LispNumber 2.0;

    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let actual =
        match res with
            | Ok v -> v.Item1

    Assert.AreEqual (expected.ToString(), actual.ToString())

[<Test>]
let TestNestedPlusEval() =
    let exp = "( + (+ 1.0 1.0) (+ 1.0 1.0))"
    let expected = LispNumber 4.0;

    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let actual =
        match res with
            | Ok v -> v.Item1

    let eStr = expected.ToString()
    let aStr = actual.ToString()
    Assert.AreEqual (eStr, aStr)

[<Test>]
let TestDefineVar() =
    let exp = "((define var 1.0) (+ var 1.0))"
    let expected = LispNumber 2.0;

    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let actual =
        match res with
            | Ok v -> v.Item1

    let eStr = expected.ToString()
    let aStr = actual.ToString()
    Assert.AreEqual (eStr, aStr)

[<Test>]
let TestDefineFunc() =
    let exp = "((define (func a) (+ a 1.0 )) (func (1.0)))"
    let expected = LispNumber 2.0;

    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let actual =
        match res with
            | Ok v -> v.Item1

    let eStr = expected.ToString()
    let aStr = actual.ToString()
    Assert.AreEqual (eStr, aStr)

[<Test>]
let TestDefineFunc2() =
    let exp = "((define (func a b) (+ a b )) (func 1.0 1.0))"
    let expected = LispNumber 2.0;

    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let actual =
        match res with
            | Ok v -> v.Item1

    let eStr = expected.ToString()
    let aStr = actual.ToString()
    Assert.AreEqual (eStr, aStr)

[<Test>]
let TestLambda() =
    let exp = "((lambda (a) (+ a 1.0)) 1.0 )"
    let expected = LispNumber 2.0;

    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let actual =
        match res with
            | Ok v -> v.Item1

    let eStr = expected.ToString()
    let aStr = actual.ToString()
    Assert.AreEqual (eStr, aStr)

[<Test>]
let TestLambda2() =
    let exp = "((lambda (a b) (+ a b)) 1.0 1.0)"
    let expected = LispNumber 2.0;

    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let actual =
        match res with
            | Ok v -> v.Item1

    let eStr = expected.ToString()
    let aStr = actual.ToString()
    Assert.AreEqual (eStr, aStr)

[<Test>]
let TestDefLambda2() =
    let exp = "((define f (lambda (a b) (+ a b))) (f 1.0 1.0))"
    let expected = LispNumber 2.0;

    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let actual =
        match res with
            | Ok v -> v.Item1

    let eStr = expected.ToString()
    let aStr = actual.ToString()
    Assert.AreEqual (eStr, aStr)

[<Test>]
let TestDefLambda2_() =
    let exp = "((define (f) (lambda (a b) (+ a b))) (f 1.0 1.0))"
    let expected = LispNumber 2.0;

    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let actual =
        match res with
            | Ok v -> v.Item1

    let eStr = expected.ToString()
    let aStr = actual.ToString()
    Assert.AreEqual (eStr, aStr)

[<Test>]
let TestVec3Parse () =
    let exp = "(Vec3 1.0 1.0 1.0)"
    let expected = LispList [LispAtom "Vec3"; LispNumber 1.0; LispNumber 1.0; LispNumber 1.0]
    let actual  = match parser exp with
                | Microsoft.FSharp.Core.Ok h -> h
    Assert.AreEqual (expected.ToString(), actual.ToString())

[<Test>]
let TestVec3Eval () =
    let exp = "(Vec3 1.0 1.0 1.0)"
    let expected = "RaytrAkkar.Raytracer.Vec3"
    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let evaluated =
        match res with
            | Ok v -> v.Item1

    let returnObject = match evaluated with
        | LispWrapper obj -> obj.ToString()

    Assert.AreEqual (expected, returnObject)

[<Test>]
let TestLambertianEval () =
    let exp = "(Lambertian (Vec3 0.1 0.2 0.5))"
    let expected = "RaytrAkkar.Raytracer.Lambertian"
    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let evaluated =
        match res with
            | Ok v -> v.Item1

    let returnObject = match evaluated with
        | LispWrapper obj -> obj.ToString()

    Assert.AreEqual (expected, returnObject)

[<Test>]
let TestSphereEval () =
    let exp = "(Sphere (Vec3 0.0 0.0 -1.0) 0.5 (Lambertian (Vec3 0.1 0.2 0.5)))"
    let expected = "RaytrAkkar.Raytracer.Sphere"
    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let evaluated =
        match res with
            | Ok v -> v.Item1

    let returnObject = match evaluated with
        | LispWrapper obj -> obj.ToString()

    Assert.AreEqual (expected, returnObject)

[<Test>]
let TestVec3DefineEval () =
    let exp = """
    ((define pos (Vec3 0.0 0.0 -1.0))
    (define albedo (Vec3 0.1 0.2 0.5))
    (Sphere pos 0.5 (Lambertian albedo)))
    """
    let expected = "RaytrAkkar.Raytracer.Sphere"
    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let evaluated =
        match res with
            | Ok v -> v.Item1

    let returnObject = match evaluated with
        | LispWrapper obj -> obj.ToString()

    Assert.AreEqual (expected, returnObject)

[<Test>]
let TestWorldEval () =
    let exp = """(World
    (Sphere (Vec3 0.0 0.0 -1.0) 0.5 (Lambertian (Vec3 0.1 0.2 0.5)))
    (Sphere (Vec3 0.0 -100.5 -1.0) 100 (Lambertian (Vec3 0.8 0.8 0.0)))
    (Sphere (Vec3 1.0 0.0 -1.0) 0.5 (Metal (Vec3 0.8 0.8 0.0) 0))
    (Sphere (Vec3 -1.0 0.0 -1.0) 0.5 (Dielectric 1.5))
    )
    """
    let expected = "RaytrAkkar.Raytracer.HitableCollection"
    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let evaluated =
        match res with
            | Ok v -> v.Item1

    let returnObject = match evaluated with
        | LispWrapper obj -> obj.ToString()

    Assert.AreEqual (expected, returnObject)

[<Test>]
let TestSimpleSceneEval () =
    let exp = """(SimpleScene (World
    (Sphere (Vec3 0.0 0.0 -1.0) 0.5 (Lambertian (Vec3 0.1 0.2 0.5)))
    (Sphere (Vec3 0.0 -100.5 -1.0) 100 (Lambertian (Vec3 0.8 0.8 0.0)))
    (Sphere (Vec3 1.0 0.0 -1.0) 0.5 (Metal (Vec3 0.8 0.8 0.0) 0))
    (Sphere (Vec3 -1.0 0.0 -1.0) 0.5 (Dielectric 1.5))
    ) (Vec3 0.0 0.0 -1.0) (Vec3 3.0 3.0 2.0)
    )
    """
    let expected = "RaytrAkkar.Raytracer.SimpleScene"
    let res = result {
        let! parsed = parser exp
        let! e = evaluator parsed
        return e
        }
    let evaluated =
        match res with
            | Ok v -> v.Item1

    let returnObject = match evaluated with
        | LispWrapper obj -> obj.ToString()

    Assert.AreEqual (expected, returnObject)

[<Test>]
let TestGetSimpleScene () =
    let exp = """(SimpleScene (World
    (Sphere (Vec3 0.0 0.0 -1.0) 0.5 (Lambertian (Vec3 0.1 0.2 0.5)))
    (Sphere (Vec3 0.0 -100.5 -1.0) 100 (Lambertian (Vec3 0.8 0.8 0.0)))
    (Sphere (Vec3 1.0 0.0 -1.0) 0.5 (Metal (Vec3 0.8 0.8 0.0) 0))
    (Sphere (Vec3 -1.0 0.0 -1.0) 0.5 (Dielectric 1.5))
    ) (Vec3 0.0 0.0 -1.0) (Vec3 3.0 3.0 2.0)
    )
    """
    let scene = GetScene.SimpleScene exp

    Assert.AreEqual ("RaytrAkkar.Raytracer.SimpleScene", scene.ToString())

[<Test>]
let TestGetSimpleSceneFail () =
    let exp = """(SimpleScene (World
    (Sphere (Vec3 0.0 0.0 -1.0) 0.5 (Lambertian (Vec3 0.1 0.2 0.5)))
    (Sphere (Vec3 0.0 -100.5 -1.0) 100 (Lambertian (Vec3 0.8 0.8 0.0)))
    (Sphere (Vec3 1.0 0.0 -1.0) 0.5 (Metal (Vec3 0.8 0.8 0.0) 0))
    (Sphere (Vec3 -1.0 0.0 -1.0) 0.5 (Dielectric 1.5))
    ) (Vec3 0.0 0.0 -1.0) (Vec3 3.0 3.0 2.0)
    """
    let scene =
        try
            sprintf "%A" (GetScene.SimpleScene exp)
        with
            | Failure msg -> "Failure Caught"

    Assert.AreEqual ("Failure Caught", scene)