/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.Collections.Generic;
using UnityEngine;

namespace HandMR
{
    public class KalmanFilter : MonoBehaviour
    {
        private HandVRSphereHand handVRSphereHand;
        private Transform thistransform;
        private Transform referencetransform;
        private Rigidbody thisrigidbody;
        private float[,] F = new float[2, 2];
        static private float[,] H = new float[1, 2] { { 1, 0 } };
        static private float[,] I = new float[2, 2] { { 1, 0 }, { 0, 1 } };
        private List<float[,]> x_hat_k_k1 = new List<float[,]>(3);
        private List<float[,]> x_hat_k_k = new List<float[,]>(3);
        private List<float[,]> P_k_k = new List<float[,]>(3);
        private List<float[,]> P_k_k1 = new List<float[,]>(3);
        private float[,] Q = new float[2, 2] { { 1, 0 }, { 0, 1 } };
        static private float[,] R = new float[1, 1] { { 100f } };
        private List<float[,]> S_k = new List<float[,]>(3);
        private List<float[,]> e_k = new List<float[,]>(3);
        private List<float[,]> K_k = new List<float[,]>(3);
        private List<float[,]> nowpos = new List<float[,]>(3);
        private List<float> sigma = new List<float>(3);
        private bool isFirstFrame = true;

        public KalmanFilter Initialize(GameObject referenceObj)
        {
            this.thistransform = this.gameObject.transform;
            this.thisrigidbody = this.gameObject.GetComponent<Rigidbody>();
            this.initparameter();
            this.referencetransform = referenceObj.transform;
            this.handVRSphereHand = referenceObj.gameObject.GetComponentInParent<HandVRSphereHand>();
            return this;
        }

        void Update()
        {
            if (handVRSphereHand.IsTrackingHand)
            {
                if (isFirstFrame)
                {
                    updatetonowpos(x_hat_k_k1);
                    isFirstFrame = false;
                }
                else
                {
                    update_parameter_all();
                    Replypos();
                }
            }
            else isFirstFrame = true;
        }
        private void initparameter()
        {
            for (int i = 0; i < 3; i++)
            {
                x_hat_k_k1.Add(new float[2, 1] { { 0 }, { 0 } });
                x_hat_k_k.Add(new float[2, 1] { { 0 }, { 0 } });
                P_k_k.Add(new float[2, 2] { { 6.4f, 4.8f }, { 4.8f, 19 } });
                P_k_k1.Add(new float[2, 2] { { 0, 0 }, { 0, 0 } });
                S_k.Add(new float[1, 1] { { 0 } });
                e_k.Add(new float[1, 1] { { 0 } });
                K_k.Add(new float[2, 1] { { 0 }, { 0 } });
                nowpos.Add(new float[2, 1] { { 0 }, { 0 } });
                sigma.Add(0f);
            }
        }
        private void updatetonowpos(List<float[,]> _nowpos)
        {
            _nowpos[0] = new float[2, 1] { { this.referencetransform.position.x }, { thisrigidbody.velocity.x } };
            _nowpos[1] = new float[2, 1] { { this.referencetransform.position.y }, { thisrigidbody.velocity.y } };
            _nowpos[2] = new float[2, 1] { { this.referencetransform.position.z }, { thisrigidbody.velocity.z } };
        }
        private void update_parameter_all()
        {
            F = new float[2, 2] { { 1, Time.deltaTime }, { 0, 1 } };
            updatetonowpos(nowpos);
            for (int i = 0; i < x_hat_k_k.Count; i++) update_parameter(i);
        }
        private void update_parameter(int i)
        {
            x_hat_k_k1[i] = Multiply(F, x_hat_k_k[i]);
            P_k_k1[i] = plus(Multiply(F, P_k_k[i], T_(F)), Q);
            S_k[i] = plus(Multiply(H, P_k_k1[i], T_(H)), R);
            K_k[i] = Multiply(P_k_k1[i], T_(H), new float[1, 1] { { 1 / S_k[i][0, 0] } });
            e_k[i] = minus(new float[1, 1] { { nowpos[i][0, 0] } }, Multiply(H, x_hat_k_k1[i]));
            sigma[i] = Mathf.Sqrt(plus(Multiply(H, P_k_k1[i], T_(H)), R)[0, 0]) / 70;
            Update_x_hat_k_k(i);
            P_k_k[i] = Multiply(minus(I, Multiply(K_k[i], H)), P_k_k1[i]);
        }
        private void Replypos()
        {
            thistransform.position = new Vector3(x_hat_k_k[0][0, 0], x_hat_k_k[1][0, 0], x_hat_k_k[2][0, 0]);
            thisrigidbody.velocity = new Vector3(x_hat_k_k[0][1, 0], x_hat_k_k[1][1, 0], x_hat_k_k[2][1, 0]);
        }
        private void Update_x_hat_k_k(int i)
        {
            float e = e_k[i][0, 0];
            if (e >= sigma[i])
            {
                x_hat_k_k[i] = plus(x_hat_k_k1[i], Multiply(K_k[i], new float[1, 1] { { sigma[i] } }));
            }
            else if (e < sigma[i] && -sigma[i] <= e)
            {
                x_hat_k_k[i] = plus(x_hat_k_k1[i], Multiply(K_k[i], e_k[i]));
            }
            else
            {
                x_hat_k_k[i] = minus(x_hat_k_k1[i], Multiply(K_k[i], new float[1, 1] { { sigma[i] } }));
            }

        }
        private float[,] Inverse2_2(float[,] _float)
        {
            var _abs = _float[0, 0] * _float[1, 1] - _float[0, 1] * _float[1, 0];
            float[,] answer = _float;
            if (_abs != 0)
            {
                answer = new float[2, 2] { { _float[1, 1] / _abs, -_float[1, 0] / _abs }, { -_float[0, 1] / _abs, _float[0, 0] / _abs } };
            }
            return answer;
        }
        float[,] T_(float[,] A)
        {
            float[,] AT = new float[A.GetLength(1), A.GetLength(0)];
            for (int i = 0; i < A.GetLength(1); i++)
            {
                for (int j = 0; j < A.GetLength(0); j++)
                {
                    AT[i, j] = A[j, i];
                }
            }
            return AT;
        }
        float[,] Multiply(float[,] A, float[,] B)
        {
            float[,] product = new float[A.GetLength(0), B.GetLength(1)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < B.GetLength(1); j++)
                {
                    for (int k = 0; k < A.GetLength(1); k++)
                    {
                        product[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return product;
        }
        float[,] plus(float[,] A, float[,] B)
        {
            float[,] product = new float[A.GetLength(0), B.GetLength(1)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < B.GetLength(1); j++)
                {
                    product[i, j] = A[i, j] + B[i, j];
                }
            }
            return product;
        }
        float[,] minus(float[,] A, float[,] B)
        {
            float[,] product = new float[A.GetLength(0), B.GetLength(1)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < B.GetLength(1); j++)
                {
                    product[i, j] = A[i, j] - B[i, j];
                }
            }
            return product;
        }
        float[,] Multiply(float[,] A, float[,] B, float[,] C)
        {
            return Multiply(Multiply(A, B), C);
        }
        static public void AddKalmanFilter(GameObject _referenceobj, GameObject _targetobj, int i)
        {
            var HandVRPosition = _referenceobj.transform.GetChild(i).gameObject.GetComponent<HandVRPosition>();
            if (HandVRPosition != null)
            {
                KalmanFilter kalmanFilter = _targetobj.transform.GetChild(i).gameObject.AddComponent<KalmanFilter>();
                kalmanFilter.Initialize(_referenceobj.transform.GetChild(i).gameObject);
            }
        }
    }

}