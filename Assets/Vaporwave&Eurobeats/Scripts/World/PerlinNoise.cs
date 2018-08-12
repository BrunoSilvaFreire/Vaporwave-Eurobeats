using UnityEngine;

public class PerlinNoise {
    
  
    public static float[,] Generate(int w, int h, int octaveCount, float amplitude, int seed){

        float[,] baseNoise = GenerateWhiteNoise(w, h, seed);
        int width = baseNoise.GetLength(0);
        int height = baseNoise.GetLength(1);

        //Um array de matrizes (geralmente usando 8, no final cada uma dessas matrizes será somada para gerar a imagem final
        float[,,] smoothNoise = new float[octaveCount, width, height];

        float persistence = 0.50f;

        for (int i = 0; i < octaveCount; i++){
            //cada loop do for gera uma nova matriz mais suave que a anteior, todas baseadas na primeira matriz de noise gerada no programa
            var smoothMatrix = GenerateSmoothNoise(baseNoise, i);
            for (int x = 0; x < smoothMatrix.GetLength(0); x++) {
                for (int y = 0; y < smoothMatrix.GetLength(1); y++) {
                    smoothNoise[i, x, y] = smoothMatrix[x, y];
                }
            }      
        }
        
        float[,] perlinNoise = new float[width,height];
        float totalAmplitude = 0.0f;

        //For feito para somar cada uma das matrizes em uma unica
        for (int octave = octaveCount - 1; octave >= 0; octave--){


            amplitude *= persistence;
            totalAmplitude += amplitude;

            for (int x = 0; x < width; x++){
                for (int y = 0; y < height; y++){
                    //Soma as matrizes em uma unica
                    perlinNoise[x,y] += smoothNoise[octave, x, y] * amplitude;
                }
            }
        }

        //normalização (traz os valores de volta para o intervalo de 0 a 1
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                perlinNoise[x, y] /= totalAmplitude;
               
            }
        }

        return perlinNoise;
    }


    //Gera uma matriz com ruido aleatório (usando uma seed, uma mesma seed sempre gera o mesmo resultado)
    static float [,] GenerateWhiteNoise(int w, int h, int seed){

        System.Random rand = new System.Random(seed);
        
        float[,] noise = new float[w,h];
        for (int y = 0; y < h; y++){
            for (int x = 0; x < w; x++) {
                noise[x, y] = (float) rand.NextDouble();
            }
        }
        return noise;
    }

    static float[,] GenerateSmoothNoise(float[,] baseNoise, int octave){

        int width = baseNoise.GetLength(0);
        int height = baseNoise.GetLength(1);

        float[,] smoothNoise = new float[width, height];

        int samplePeriod = 1 << octave;//"move" o "1" para a esquerda. ex: octave = 1:  1 << 1 -> 10(binário) = 2
                                       //ex2: octave = 2: 1 << 2 -> 100(binário) = 4
                                       //ex3: octave = 3: 1 << 3 -> 1000(binário) = 8
        float sampleFrequency = 1.0f / samplePeriod;

        //Sinceramente não faço ideia do que essa maluquice abaixo faz
        for (int i = 0; i < width; i++){
            int sample_i0 = (i / samplePeriod) * samplePeriod;
            int sample_i1 = (sample_i0 + samplePeriod) % width;
            float horizontal_blend = (i - sample_i0) * sampleFrequency;

            for (int j = 0; j < height; j++){
                int sample_j0 = (j / samplePeriod) * samplePeriod;
                int sample_j1 = (sample_j0 + samplePeriod) % height;
                float vertical_blend = (j - sample_j0) * sampleFrequency;


                float top = Mathf.Lerp(baseNoise[sample_i0, sample_j0], baseNoise[sample_i1, sample_j0], horizontal_blend);
                float bottom = Mathf.Lerp(baseNoise[sample_i0, sample_j1], baseNoise[sample_i1, sample_j1], horizontal_blend);


                smoothNoise[i, j] = Mathf.Lerp(top, bottom, vertical_blend);
            }
        }

        return smoothNoise;
    }
}
