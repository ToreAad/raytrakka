namespace RaytrAkkar.Lisp
open Parser
open LispTypes
open Evaluator
open ResultMonad

module GetScene =
    let SimpleScene (src: string) : RaytrAkkar.Raytracer.SimpleScene =
        let res = result {
            let! parsed = parser src
            let! e = evaluator parsed
            return e
            }
        let evaluated =
            match res with
                | Ok v -> v.Item1

        let returnObject = match evaluated with
            | LispWrapper obj -> match obj with
                                 | :? RaytrAkkar.Raytracer.SimpleScene as typed -> typed
        returnObject
