/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
public class UnityUnit
{

    public float ParentLossyScale { get; }
    public UnityEngine.Vector3 ParentPosition { get; }

    public UnityUnit(float parentLossyScale, UnityEngine.Vector3 parentPosition)
    {
        this.ParentLossyScale = parentLossyScale;
        this.ParentPosition = parentPosition;
    }

    public float ConvertFromAtomicLength(double atomicLength)
        => this.ParentLossyScale * LengthScaleFromAtomic * (float)atomicLength;
    public double ConvertToAtomicLength(float unityLength)
        => unityLength / (this.ParentLossyScale * LengthScaleFromAtomic);
    public UnityEngine.Vector3 ConvertFromAtomicPosition(VRMD.PureC.Math.Vector3 atomicVector)
        => new UnityEngine.Vector3(
            this.ConvertFromAtomicLength(atomicVector.X),
            this.ConvertFromAtomicLength(atomicVector.Y),
            this.ConvertFromAtomicLength(atomicVector.Z)
        ) + this.ParentPosition;
    public VRMD.PureC.Math.Vector3 ConvertToAtomicPosition(UnityEngine.Vector3 atomicVector)
        => new VRMD.PureC.Math.Vector3(
            this.ConvertToAtomicLength(atomicVector.x - this.ParentPosition.x),
            this.ConvertToAtomicLength(atomicVector.y - this.ParentPosition.y),
            this.ConvertToAtomicLength(atomicVector.z - this.ParentPosition.z)
        );

    public float ConvertFromAtomicMass(double atomicMass)
            => MassScaleFromAtomic * (float)atomicMass;
    public double ConvertToAtomicMass(float unityMass)
        => unityMass / MassScaleFromAtomic;

    public float ConvertFromAtomicTime(double atomicTime)
        => TimeScaleFromAtomic * (float)atomicTime;
    public double ConvertToAtomicTime(float unityTime)
        => unityTime / TimeScaleFromAtomic;

    public float ConvertFromAtomicVelocity(double atomicVelocity)
        => this.ParentLossyScale * VelocityScaleFromAtomic * (float)atomicVelocity;
    public double ConvertToAtomicVelocity(float unityVelocity)
        => unityVelocity / (this.ParentLossyScale * VelocityScaleFromAtomic);
    public UnityEngine.Vector3 ConvertFromAtomicVelocity(VRMD.PureC.Math.Vector3 atomicVector)
        => new UnityEngine.Vector3(
            this.ConvertFromAtomicVelocity(atomicVector.X),
            this.ConvertFromAtomicVelocity(atomicVector.Y),
            this.ConvertFromAtomicVelocity(atomicVector.Z)
        );
    public VRMD.PureC.Math.Vector3 ConvertToAtomicVelocity(UnityEngine.Vector3 atomicVector)
        => new VRMD.PureC.Math.Vector3(
            this.ConvertToAtomicVelocity(atomicVector.x),
            this.ConvertToAtomicVelocity(atomicVector.y),
            this.ConvertToAtomicVelocity(atomicVector.z)
        );

    public float ConvertFromAtomicForce(double atomicForce)
        => this.ParentLossyScale * ForceScaleFromAtomic * (float)atomicForce;
    public double ConvertToAtomicForce(float unityForce)
        => unityForce / (this.ParentLossyScale * ForceScaleFromAtomic);
    public UnityEngine.Vector3 ConvertFromAtomicForce(VRMD.PureC.Math.Vector3 atomicForce)
        => new UnityEngine.Vector3(
            this.ConvertFromAtomicForce(atomicForce.X),
            this.ConvertFromAtomicForce(atomicForce.Y),
            this.ConvertFromAtomicForce(atomicForce.Z)
        );
    public VRMD.PureC.Math.Vector3 ConvertToAtomicForce(UnityEngine.Vector3 atomicForce)
        => new VRMD.PureC.Math.Vector3(
            this.ConvertToAtomicForce(atomicForce.x),
            this.ConvertToAtomicForce(atomicForce.y),
            this.ConvertToAtomicForce(atomicForce.z)
        );

    public float ConvertAtomicScaleToUnityLocalScale(double atomicScale)
    {
        var lossyScale = this.ConvertFromAtomicLength(atomicScale);
        return lossyScale / this.ParentLossyScale;
    }

    private const float amu = 1.66054e-27f;
    private const float LengthScaleFromAtomic = 1e10f;
    private const float MassScaleFromAtomic = 1.0f / 12f / amu;
    public const float TimeScaleFromAtomic = 3e14f;
    public const float VelocityScaleFromAtomic = LengthScaleFromAtomic / TimeScaleFromAtomic;
    public const float ForceScaleFromAtomic
       = MassScaleFromAtomic * LengthScaleFromAtomic / (TimeScaleFromAtomic * TimeScaleFromAtomic);


}
