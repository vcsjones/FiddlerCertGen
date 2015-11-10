namespace VCSJones.FiddlerCertGen

[<StructuralComparisonAttribute; StructuralEqualityAttribute>]
type Algorithm =
    private {
        name : string
    }
    static member RSA = {name = "RSA"}
    static member ECDSA_P256 = {name = "ECDSA_P256"}
    static member ECDSA_P384 = {name = "ECDSA_P384"}
    static member ECDSA_P521 = {name = "ECDSA_P521"}

    static member TryParse value =
        match value with
        | "RSA" -> Success Algorithm.RSA
        | "ECDSA_P256" -> Success Algorithm.ECDSA_P256
        | "ECDSA_P384" -> Success Algorithm.ECDSA_P384
        | "ECDSA_P521" -> Success Algorithm.ECDSA_P521
        | _ -> Fail

    override this.ToString() = this.name
    static member op_Equality (left : Algorithm, right : Algorithm) = left = right
    static member op_Inequality (left : Algorithm, right : Algorithm) = left <> right