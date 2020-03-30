using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2_0325
{
    class BP_Learn
    {
        /* FOR BPA START */
        //const
        const int Input_Neuron  = 4;
        const int Output_Neuron = 1;
        const int Number_Layer  = 2; //2
        const int Hd_L_Number   = 10;//hidden layer of neuron number
        const int Number_Neurons = Hd_L_Number * Number_Layer + Output_Neuron; //TEMP
        const int Get_5days = 5; // HTML COUNT -->  5 days?
        const int Get_20days = 20;
        const int Get_60days = 60;

        const int Get_days = Get_60days;

        //For Version2
        const int Input_Neuron_v2 = 3;
        double[] Input_v2 = new double[Get_days * Input_Neuron_v2];
        double[] T_Input_v2 = new double[Input_Neuron_v2];

        //int 
        int Bias = 1;
        int bnc = 0;
        int inc = 0;
        int k = 0;
        int jump = 0;
        int carry = 0;
        int small_jump = 0;
        int Iteration = 0;
        
        int Epoch = 1000000; // loop

        int New_Lable = Number_Neurons - Output_Neuron;
        int Lable = Number_Neurons - Output_Neuron - Hd_L_Number;
        
        double RMSE = 0;
        double L_N_G = 0.4;
        
        double[] Input      = new double[Get_days * Input_Neuron];
        double[] Sigmoid    = new double[100];
        double[] Delta      = new double[100];
        double[] Sum        = new double[100];
        
        double[] Sum_Output     = new double[10];
        double[] Sigmoid_Output = new double[10];
        double[] Delta_Output   = new double[10];
        double[] Error          = new double[10];
        double[] Error_Result   = new double[10];
        double[] Error_add      = new double[10];
        
        double[] Bias_Weight   = new double[Number_Neurons];
        
        double[] Weight_Input_Layer  = new double[100]; //Input_Neuron * Hidden_Layer 1layer
        double[] Weight_Output_Layer = new double[100]; //Output Neuron * Hidden_Layer Last layer
        double[,] Weight_Layer = new double[Number_Layer, Hd_L_Number* Hd_L_Number* Number_Layer];
        double[,] Target_t = new double[Get_days, Output_Neuron]; // ?
        
        int[] Hidden_Layer = new int[10];
        /* FOR BPA END */

        /* FOR BPA TEST START */
        double[] T_Input         = new double[Input_Neuron];
        double[] T_Sum           = new double[100];
        double[] T_Sigmoid       = new double[100];
        double[] T_Output_Sum    = new double[100];
        double[] T_Output_Sigmoid = new double[100];
        /* FOR BPA TEST END */

        //Random class for Weight,Bias
        Random ran = new Random();

        // temp value
        int max_count = 0;
        int digits = 0;
        int max_count_tv = 0;
        int digits_tv = 0;

        /*BPA Learn*/
//1.시가 2.고가 3.저가 4.거래량 5.종가(타겟)
        public double BP_START_STOCK(ref stock_[] bp_stock)
        {
            //Initial
            Global_days GG = new Global_days();
            stock_[] stock_bp = new stock_[GG._days];
            stock_bp = bp_stock; //ref

            //Bias, Hidden Neuron Weight Set
            for (int i = 0; i < Number_Neurons; i++)
            {
                Bias_Weight[i] = ran.NextDouble();
            }
            for (int i = 0; i < Input_Neuron * Hd_L_Number; i++)
            {
                Weight_Input_Layer[i] = ran.NextDouble();
            }
            for (int i = 0; i < Number_Layer; i++)
            {
                for (int j = 0; j < Hd_L_Number * Hd_L_Number; j++)
                {
                    Weight_Layer[i, j] = ran.NextDouble();
                }
            }
            for (int i = 0; i < Output_Neuron * Hd_L_Number; i++)
            {
                Weight_Output_Layer[i] = ran.NextDouble();
            }

            //max count get
            for (int i = 0; i < Get_days; i++)
            {
                if (max_count < stock_bp[i].s_dhp_int)
                {
                    max_count = stock_bp[i].s_dhp_int;
                }
            }
            do
            {
                max_count = (max_count / 10);
                digits++;
            } while (max_count > 0);

            for (int i = 0; i < Get_days; i++)
            {
                if (max_count_tv < stock_bp[i].s_dtv_int)
                {
                    max_count_tv = stock_bp[i].s_dtv_int;
                }
            }
            do
            {
                max_count_tv = (max_count_tv / 10);
                digits_tv++;
            } while (max_count_tv > 0);

            //Input Set
            for (int i = 0; i < Get_days; i++)
            {
                
                Input[i + 0] = stock_bp[i].s_dmp_int;
                Input[i + 1] = stock_bp[i].s_dhp_int;
                Input[i + 2] = stock_bp[i].s_dlp_int;
                Input[i + 3] = stock_bp[i].s_dtv_int;

                Input[i + 0] /= Math.Pow(10, digits);
                Input[i + 1] /= Math.Pow(10, digits);
                Input[i + 2] /= Math.Pow(10, digits);
                Input[i + 3] /= Math.Pow(10, digits_tv);
            }

            for (int i = 0; i < Get_days; i++)
            {
                for (int j = 0; j < Output_Neuron; j++)
                {
                    Target_t[i, j] = stock_bp[i].s_dcp_int;

                    Target_t[i, j] /= Math.Pow(10, digits);
                }
            }

            //Output Set

            //BPA Start
            while (Epoch-- > 0)
            {
                /*Input - Hidden Layer[0] 사이 Sum,Sigmoid,Delta */
                for (int i = 0; i < Input_Neuron; i++)
                {
                    for (int j = 0; j < Input_Neuron; j++)
                    {
                        Sum[i] += Input[j + bnc * Input_Neuron] * Weight_Input_Layer[inc];
                        ++inc;
                    }
                    Sum[i] += (Bias * Bias_Weight[i]);
                    Sigmoid[i] = (1.0 / (1.0 + Math.Exp(-Sum[i])));
                }
                inc = 0;

                /*Hidden Layer 사이의 Sum, Sigmoid*/
                for (int i = Number_Layer - 1; i > 0; i--)
                {
                    k += Hd_L_Number;
                    //ex) 20,21,22,23,24 / 15,16,17,18,19 / ...
                    for (int j = New_Lable - (Hd_L_Number + jump); j < New_Lable - jump; j++)
                    {
                        //ex) 25-(5+5*k) -> n=20-5k; n < 25-5k; n++ -> 20,21,22,23,24 / 15,16,17,18,19 / ....
                        for (int n = New_Lable - (Hd_L_Number + k); n < New_Lable - k; n++)
                        {
                            Sum[j] += (Sigmoid[n] * Weight_Layer[i - 1, inc]);
                            ++inc;
                        }
                        Sum[j] += (Bias * Bias_Weight[j]);
                        Sigmoid[j] = (1.0 / (1.0 + Math.Exp(-Sum[j])));
                    }
                    inc = 0;
                    jump += Hd_L_Number;
                }
                jump = 0;
                k = 0;

                /*	Output Layer와 연결된 Hidden Layer이용하여 Output Sum,Sigmoid	*/
                for (int i = 0; i < Output_Neuron; ++i)
                {
                    for (int j = Lable; j < New_Lable; j++)
                    {
                        Sum_Output[i] += (Sigmoid[j] * Weight_Output_Layer[inc]);
                        inc++;
                    }
                    Sum_Output[i] += (Bias * Bias_Weight[New_Lable + i]);
                    Sigmoid_Output[i] = (1.0 / (1.0 + Math.Exp(-Sum_Output[i])));
                    Delta_Output[i] = (Sigmoid_Output[i] * (1 - Sigmoid_Output[i])) * (Target_t[bnc, i] - Sigmoid_Output[i]);

                    /*Target 값 설정 주의*/
                    for (int j = Lable; j < New_Lable; ++j)
                    {
                        Delta[j] += (Sigmoid[j] * (1 - Sigmoid[j]) * Weight_Output_Layer[carry] * Delta_Output[i]);
                        ++carry;
                    }
                }
                inc = 0;
                carry = 0;

                /*Hidden Layer들 사이의 Delta*/
                for (int i = Number_Layer - 1; i > 0; --i)
                {
                    carry += Hd_L_Number;
                    //ex) 30 - (10+jump)  < 25 - jump -> 1. 20 < 25 2. 15 < 20 3.10 < 15
                    for (int z = New_Lable - (2 * Hd_L_Number + jump); z < New_Lable - Hd_L_Number - jump; z++)
                    {
                        //ex) 30 - carry < 30 - jump  1. 25 < 30 2. 20 < 25 ...
                        for (int j = (New_Lable - carry); j < New_Lable - jump; j++)
                        {
                            Delta[z] += (Sigmoid[z] * (1 - Sigmoid[z])) * Delta[j] * Weight_Layer[i - 1, inc + small_jump];
                            small_jump += Hd_L_Number;
                        }
                        small_jump = 0;
                        jump += Hd_L_Number;
                        inc++;
                    }
                }
                carry = 0;
                inc = 0;
                jump = 0;

                /*Weight 갱신*/
                //Bias 부분
                for (int i = 0; i < New_Lable; ++i) Bias_Weight[i] = (L_N_G * Delta[i] * Bias) + Bias_Weight[i];
                for (int i = New_Lable; i < Number_Neurons; ++i) Bias_Weight[i] = (L_N_G * Delta_Output[i - New_Lable] * Bias) + Bias_Weight[i];

                //Input <---> Hidden Layer 1층 부분
                //ex) 5 * 5 -> 25
                for (int i = 0; i < (Input_Neuron * Hd_L_Number); ++i)
                {
                    carry = i % Input_Neuron; //Input 2개 일때 (l--> 0 1 0 1)
                    if (i > 0)
                    {                             //i--> 0 1 2 3 4 5 6 7  => l --> 0 1 0 1 0 1 0 1
                        if (carry == 0) ++k;  //K--> 0 0 1 1 2 2 3 3
                    }
                    Weight_Input_Layer[i] = (L_N_G * Delta[k] * Input[carry + bnc * Input_Neuron]) + Weight_Input_Layer[i];
                }
                carry = 0;
                k = 0;

                /*Hidden Layer 사이의 Weight 갱신*/
                for (int i = (Number_Layer - 1); i > 0; --i)
                {
                    carry += Hd_L_Number;
                    //ex) 1. 25 - 5 - 5 < 25 - 5  2. 25 - 10 -5 < 25 - 10
                    for (int j = (New_Lable - carry - Hd_L_Number); j < (New_Lable - carry); ++j)
                    {
                        //ex) 1. 25 - 5 < 25 - 0  2. 25-10 < 25-5 ... 
                        for (int k = (New_Lable - carry); k < (New_Lable - jump); ++k)
                        {
                            Weight_Layer[i - 1, inc] = (L_N_G * Delta[k] * Sigmoid[j]) + Weight_Layer[i - 1, inc];
                            ++inc;
                        }
                    }
                    jump += Hidden_Layer[i];
                    inc = 0;
                }
                inc = 0;
                jump = 0;
                carry = 0;

                //Hidden Layer(마지막 층) <---> Output Layer
                //ex) 1.  0 < 5*5
                for (int i = 0; i < (Output_Neuron * Hd_L_Number); ++i)
                {
                    carry = i % Hd_L_Number;  // 0 1 2 3 4   0 1 2 3 4

                    if (i > 0 && ((i % Hd_L_Number) == 0)) ++k; //ex) i: 5->0 10->0 15->0 20->0

                    Weight_Output_Layer[i] = (L_N_G * Delta_Output[k] * Sigmoid[carry + Lable]) + Weight_Output_Layer[i];
                }
                carry = 0;
                k = 0;

                /*Delta += 사용하였기 때문에 초기화 해주어야 함*/
                for (int i = 0; i < New_Lable; ++i) Delta[i] = 0;
                for (int i = 0; i < Output_Neuron; ++i) Delta_Output[i] = 0;

                /*Sum += 문법 초기화*/
                for (int i = 0; i < New_Lable; ++i) Sum[i] = 0;
                for (int i = 0; i < Output_Neuron; ++i) Sum_Output[i] = 0;

                /*최종 Error 값 구하는 곳*/
                //Mean Square Error 적용해보기
                //RMSE =  Root *( (1.0 / n) * Sigma(i) * pow((Target_Vector(i) - Output(i)) , 2)  )
                //루트 --> sqrt(실수)     ,    제곱 --> pow(a , 2)
                for (int i = 0; i < Output_Neuron; ++i)
                {
                    Error[i] = (Target_t[bnc, i] - Sigmoid_Output[i]);
                    RMSE += ((1.0 / Output_Neuron) * Math.Pow(Error[i], 2));
                    Error_add[i] += Math.Abs(Error[i]);

                }
                RMSE = Math.Sqrt(RMSE);

                ++bnc;
                ++Iteration;
                if (bnc == Get_days) bnc = 0;

                if ((Iteration % Get_days) == 0)
                {
                    RMSE = 0;
                    for (int i = 0; i < Output_Neuron; ++i)
                    {
                        Error_Result[i] = (Error_add[i] / Get_days);
                        Error_add[i] = 0;
                    }
                }
            }//while(Epoch-- > 0)

            //TEST OUTPUT(TEXT Result)
            /*Test할 Input 값 입력*/
            bnc = 0;
#if false
            for (int i = 0; i < Input_Neuron; i++)
            {
                for (int j = 0; j < Get_days; j++)
                {
                    //ex) 0,4,8,12,16   ||  0 4 8 12 16 
                    //ex) 1,5,9,13,17   ||  1 5 9 13
                    T_Input[i] += Input[j*Input_Neuron + i];
                }
                T_Input[i] = T_Input[i] / Get_days;
            }
#endif
            //1.시가 2.고가 3.저가 4.거래량 5.종가(타겟)
            T_Input[0] = stock_bp[0].s_dmp_int;
            T_Input[1] = stock_bp[0].s_dhp_int;
            T_Input[2] = stock_bp[0].s_dlp_int;
            T_Input[3] = stock_bp[0].s_dtv_int;

            T_Input[0] /= Math.Pow(10, digits);
            T_Input[1] /= Math.Pow(10, digits);
            T_Input[2] /= Math.Pow(10, digits);
            T_Input[3] /= Math.Pow(10, digits_tv);

            /*Input - Hidden Layer[0] 사이 Sum,Sigmoid,Delta */
            for (int i = 0; i < Hd_L_Number; ++i)
            {
                for (int j = 0; j < Input_Neuron; ++j)
                {
                    T_Sum[i] += (T_Input[j + bnc * 2] * Weight_Input_Layer[inc]);
                    ++inc;
                }
                T_Sum[i] += (Bias * Bias_Weight[i]);
                T_Sigmoid[i] = (1.0 / (1.0 + Math.Exp(-T_Sum[i])));
            }
            inc = 0;

            /*Hidden Layer들 사이의 Sum, Sigmoid*/
            for (int i = 0; i < (Number_Layer - 1); ++i)
            {
                carry += Hd_L_Number;
                for (int j = carry; j < carry + Hd_L_Number; ++j)
                {
                    for (int k = jump; k < carry; ++k)
                    {
                        T_Sum[j] += (T_Sigmoid[k] * Weight_Layer[i, inc]);
                        ++inc;
                    }
                    T_Sum[j] += (Bias * Bias_Weight[j]);
                    T_Sigmoid[j] = (1.0 / (1.0 + Math.Exp(-T_Sum[j])));
                }
                inc = 0;
                jump += Hd_L_Number;
            }
            jump = 0;
            carry = 0;

            /*	Output Layer와 연결된 Hidden Layer이용하여 Output Sum,Sigmoid	*/
            for (int i = 0; i < Output_Neuron; ++i)
            {
                for (int j = Lable; j < New_Lable; ++j)
                {
                    T_Output_Sum[i] += (T_Sigmoid[j] * Weight_Output_Layer[inc]);
                    ++inc;
                }
                T_Output_Sum[i] += (Bias * Bias_Weight[New_Lable + i]);
                T_Output_Sigmoid[i] = (1.0 / (1.0 + Math.Exp(-T_Output_Sum[i])));
            }
            inc = 0;

            return Math.Abs(T_Output_Sigmoid[0]) * Math.Pow(10, digits);
        } //BP_START


 /*
 * BP STOCK VERSION2
 * TRANSACTION VOLUME DELETE
 */
        public double BP_START_STOCK_VERSION2(ref stock_v2[] bp_stock_v2)
        {
            //Initial
            Global_days GG = new Global_days();
            stock_v2[] stock_bp = new stock_v2[GG._days];
            stock_bp = bp_stock_v2; //ref

            //Bias, Hidden Neuron Weight Set
            for (int i = 0; i < Number_Neurons; i++)
            {
                Bias_Weight[i] = ran.NextDouble();
            }
            for (int i = 0; i < Input_Neuron_v2 * Hd_L_Number; i++)
            {
                Weight_Input_Layer[i] = ran.NextDouble();
            }
            for (int i = 0; i < Number_Layer; i++)
            {
                for (int j = 0; j < Hd_L_Number * Hd_L_Number; j++)
                {
                    Weight_Layer[i, j] = ran.NextDouble();
                }
            }
            for (int i = 0; i < Output_Neuron * Hd_L_Number; i++)
            {
                Weight_Output_Layer[i] = ran.NextDouble();
            }

            //max count get
            for (int i = 0; i < Get_days; i++)
            {
                if (max_count < stock_bp[i].s_dhp_int)
                {
                    max_count = stock_bp[i].s_dhp_int;
                }
            }
            do
            {
                max_count = (max_count / 10);
                digits++;
            } while (max_count > 0);
#if false
            for (int i = 0; i < Get_days; i++)
            {
                if (max_count_tv < stock_bp[i].s_dtv_int)
                {
                    max_count_tv = stock_bp[i].s_dtv_int;
                }
            }
            do
            {
                max_count_tv = (max_count_tv / 10);
                digits_tv++;
            } while (max_count_tv > 0);
#endif
            //Input Set
            for (int i = 0; i < Get_days; i++)
            {

                Input_v2[i + 0] = stock_bp[i].s_dmp_int;
                Input_v2[i + 1] = stock_bp[i].s_dhp_int;
                Input_v2[i + 2] = stock_bp[i].s_dlp_int;

                Input_v2[i + 0] /= Math.Pow(10, digits);
                Input_v2[i + 1] /= Math.Pow(10, digits);
                Input_v2[i + 2] /= Math.Pow(10, digits);
            }

            for (int i = 0; i < Get_days; i++)
            {
                for (int j = 0; j < Output_Neuron; j++)
                {
                    Target_t[i, j] = stock_bp[i].s_dcp_int;

                    Target_t[i, j] /= Math.Pow(10, digits);
                }
            }

            //Output Set

            //BPA Start
            while (Epoch-- > 0)
            {
                /*Input - Hidden Layer[0] 사이 Sum,Sigmoid,Delta */
                for (int i = 0; i < Input_Neuron_v2; i++)
                {
                    for (int j = 0; j < Input_Neuron_v2; j++)
                    {
                        Sum[i] += Input_v2[j + bnc * Input_Neuron_v2] * Weight_Input_Layer[inc];
                        ++inc;
                    }
                    Sum[i] += (Bias * Bias_Weight[i]);
                    Sigmoid[i] = (1.0 / (1.0 + Math.Exp(-Sum[i])));
                }
                inc = 0;

                /*Hidden Layer 사이의 Sum, Sigmoid*/
                for (int i = Number_Layer - 1; i > 0; i--)
                {
                    k += Hd_L_Number;
                    //ex) 20,21,22,23,24 / 15,16,17,18,19 / ...
                    for (int j = New_Lable - (Hd_L_Number + jump); j < New_Lable - jump; j++)
                    {
                        //ex) 25-(5+5*k) -> n=20-5k; n < 25-5k; n++ -> 20,21,22,23,24 / 15,16,17,18,19 / ....
                        for (int n = New_Lable - (Hd_L_Number + k); n < New_Lable - k; n++)
                        {
                            Sum[j] += (Sigmoid[n] * Weight_Layer[i - 1, inc]);
                            ++inc;
                        }
                        Sum[j] += (Bias * Bias_Weight[j]);
                        Sigmoid[j] = (1.0 / (1.0 + Math.Exp(-Sum[j])));
                    }
                    inc = 0;
                    jump += Hd_L_Number;
                }
                jump = 0;
                k = 0;

                /*	Output Layer와 연결된 Hidden Layer이용하여 Output Sum,Sigmoid	*/
                for (int i = 0; i < Output_Neuron; ++i)
                {
                    for (int j = Lable; j < New_Lable; j++)
                    {
                        Sum_Output[i] += (Sigmoid[j] * Weight_Output_Layer[inc]);
                        inc++;
                    }
                    Sum_Output[i] += (Bias * Bias_Weight[New_Lable + i]);
                    Sigmoid_Output[i] = (1.0 / (1.0 + Math.Exp(-Sum_Output[i])));
                    Delta_Output[i] = (Sigmoid_Output[i] * (1 - Sigmoid_Output[i])) * (Target_t[bnc, i] - Sigmoid_Output[i]);

                    /*Target 값 설정 주의*/
                    for (int j = Lable; j < New_Lable; ++j)
                    {
                        Delta[j] += (Sigmoid[j] * (1 - Sigmoid[j]) * Weight_Output_Layer[carry] * Delta_Output[i]);
                        ++carry;
                    }
                }
                inc = 0;
                carry = 0;

                /*Hidden Layer들 사이의 Delta*/
                for (int i = Number_Layer - 1; i > 0; --i)
                {
                    carry += Hd_L_Number;
                    //ex) 30 - (10+jump)  < 25 - jump -> 1. 20 < 25 2. 15 < 20 3.10 < 15
                    for (int z = New_Lable - (2 * Hd_L_Number + jump); z < New_Lable - Hd_L_Number - jump; z++)
                    {
                        //ex) 30 - carry < 30 - jump  1. 25 < 30 2. 20 < 25 ...
                        for (int j = (New_Lable - carry); j < New_Lable - jump; j++)
                        {
                            Delta[z] += (Sigmoid[z] * (1 - Sigmoid[z])) * Delta[j] * Weight_Layer[i - 1, inc + small_jump];
                            small_jump += Hd_L_Number;
                        }
                        small_jump = 0;
                        jump += Hd_L_Number;
                        inc++;
                    }
                }
                carry = 0;
                inc = 0;
                jump = 0;

                /*Weight 갱신*/
                //Bias 부분
                for (int i = 0; i < New_Lable; ++i) Bias_Weight[i] = (L_N_G * Delta[i] * Bias) + Bias_Weight[i];
                for (int i = New_Lable; i < Number_Neurons; ++i) Bias_Weight[i] = (L_N_G * Delta_Output[i - New_Lable] * Bias) + Bias_Weight[i];

                //Input <---> Hidden Layer 1층 부분
                //ex) 5 * 5 -> 25
                for (int i = 0; i < (Input_Neuron_v2 * Hd_L_Number); ++i)
                {
                    carry = i % Input_Neuron_v2; //Input 2개 일때 (l--> 0 1 0 1)
                    if (i > 0)
                    {                             //i--> 0 1 2 3 4 5 6 7  => l --> 0 1 0 1 0 1 0 1
                        if (carry == 0) ++k;  //K--> 0 0 1 1 2 2 3 3
                    }
                    Weight_Input_Layer[i] = (L_N_G * Delta[k] * Input[carry + bnc * Input_Neuron_v2]) + Weight_Input_Layer[i];
                }
                carry = 0;
                k = 0;

                /*Hidden Layer 사이의 Weight 갱신*/
                for (int i = (Number_Layer - 1); i > 0; --i)
                {
                    carry += Hd_L_Number;
                    //ex) 1. 25 - 5 - 5 < 25 - 5  2. 25 - 10 -5 < 25 - 10
                    for (int j = (New_Lable - carry - Hd_L_Number); j < (New_Lable - carry); ++j)
                    {
                        //ex) 1. 25 - 5 < 25 - 0  2. 25-10 < 25-5 ... 
                        for (int k = (New_Lable - carry); k < (New_Lable - jump); ++k)
                        {
                            Weight_Layer[i - 1, inc] = (L_N_G * Delta[k] * Sigmoid[j]) + Weight_Layer[i - 1, inc];
                            ++inc;
                        }
                    }
                    jump += Hidden_Layer[i];
                    inc = 0;
                }
                inc = 0;
                jump = 0;
                carry = 0;

                //Hidden Layer(마지막 층) <---> Output Layer
                //ex) 1.  0 < 5*5
                for (int i = 0; i < (Output_Neuron * Hd_L_Number); ++i)
                {
                    carry = i % Hd_L_Number;  // 0 1 2 3 4   0 1 2 3 4

                    if (i > 0 && ((i % Hd_L_Number) == 0)) ++k; //ex) i: 5->0 10->0 15->0 20->0

                    Weight_Output_Layer[i] = (L_N_G * Delta_Output[k] * Sigmoid[carry + Lable]) + Weight_Output_Layer[i];
                }
                carry = 0;
                k = 0;

                /*Delta += 사용하였기 때문에 초기화 해주어야 함*/
                for (int i = 0; i < New_Lable; ++i) Delta[i] = 0;
                for (int i = 0; i < Output_Neuron; ++i) Delta_Output[i] = 0;

                /*Sum += 문법 초기화*/
                for (int i = 0; i < New_Lable; ++i) Sum[i] = 0;
                for (int i = 0; i < Output_Neuron; ++i) Sum_Output[i] = 0;

                /*최종 Error 값 구하는 곳*/
                //Mean Square Error 적용해보기
                //RMSE =  Root *( (1.0 / n) * Sigma(i) * pow((Target_Vector(i) - Output(i)) , 2)  )
                //루트 --> sqrt(실수)     ,    제곱 --> pow(a , 2)
                for (int i = 0; i < Output_Neuron; ++i)
                {
                    Error[i] = (Target_t[bnc, i] - Sigmoid_Output[i]);
                    RMSE += ((1.0 / Output_Neuron) * Math.Pow(Error[i], 2));
                    Error_add[i] += Math.Abs(Error[i]);

                }
                RMSE = Math.Sqrt(RMSE);

                ++bnc;
                ++Iteration;
                if (bnc == Get_days) bnc = 0;

                if ((Iteration % Get_days) == 0)
                {
                    RMSE = 0;
                    for (int i = 0; i < Output_Neuron; ++i)
                    {
                        Error_Result[i] = (Error_add[i] / Get_days);
                        Error_add[i] = 0;
                    }
                }
            }//while(Epoch-- > 0)

            //TEST OUTPUT(TEXT Result)
            /*Test할 Input 값 입력*/
            bnc = 0;
#if false
            for (int i = 0; i < Input_Neuron; i++)
            {
                for (int j = 0; j < Get_days; j++)
                {
                    //ex) 0,4,8,12,16   ||  0 4 8 12 16 
                    //ex) 1,5,9,13,17   ||  1 5 9 13
                    T_Input[i] += Input[j*Input_Neuron + i];
                }
                T_Input[i] = T_Input[i] / Get_days;
            }
#endif
            //1.시가 2.고가 3.저가 4.거래량 5.종가(타겟)
            T_Input_v2[0] = stock_bp[0].s_dmp_int;
            T_Input_v2[1] = stock_bp[0].s_dhp_int;
            T_Input_v2[2] = stock_bp[0].s_dlp_int;

            T_Input_v2[0] /= Math.Pow(10, digits);
            T_Input_v2[1] /= Math.Pow(10, digits);
            T_Input_v2[2] /= Math.Pow(10, digits);

            /*Input - Hidden Layer[0] 사이 Sum,Sigmoid,Delta */
            for (int i = 0; i < Hd_L_Number; ++i)
            {
                for (int j = 0; j < Input_Neuron_v2; ++j)
                {
                    T_Sum[i] += (T_Input[j + bnc * 2] * Weight_Input_Layer[inc]);
                    ++inc;
                }
                T_Sum[i] += (Bias * Bias_Weight[i]);
                T_Sigmoid[i] = (1.0 / (1.0 + Math.Exp(-T_Sum[i])));
            }
            inc = 0;

            /*Hidden Layer들 사이의 Sum, Sigmoid*/
            for (int i = 0; i < (Number_Layer - 1); ++i)
            {
                carry += Hd_L_Number;
                for (int j = carry; j < carry + Hd_L_Number; ++j)
                {
                    for (int k = jump; k < carry; ++k)
                    {
                        T_Sum[j] += (T_Sigmoid[k] * Weight_Layer[i, inc]);
                        ++inc;
                    }
                    T_Sum[j] += (Bias * Bias_Weight[j]);
                    T_Sigmoid[j] = (1.0 / (1.0 + Math.Exp(-T_Sum[j])));
                }
                inc = 0;
                jump += Hd_L_Number;
            }
            jump = 0;
            carry = 0;

            /*	Output Layer와 연결된 Hidden Layer이용하여 Output Sum,Sigmoid	*/
            for (int i = 0; i < Output_Neuron; ++i)
            {
                for (int j = Lable; j < New_Lable; ++j)
                {
                    T_Output_Sum[i] += (T_Sigmoid[j] * Weight_Output_Layer[inc]);
                    ++inc;
                }
                T_Output_Sum[i] += (Bias * Bias_Weight[New_Lable + i]);
                T_Output_Sigmoid[i] = (1.0 / (1.0 + Math.Exp(-T_Output_Sum[i])));
            }
            inc = 0;

            return Math.Abs(T_Output_Sigmoid[0]) * Math.Pow(10, digits);
        }
    } //CLASS
}
