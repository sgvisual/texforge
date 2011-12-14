﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace texforge.Generators
{
    public class PerlinNoise : Generator
    {
        //protected texforge_definitions.Settings.Color startColor = new texforge_definitions.Settings.Color("Start Color", new texforge_definitions.Types.Color(Color.White), new texforge_definitions.Types.Color(Color.Black), new texforge_definitions.Types.Color(Color.Black), new texforge_definitions.Types.Color(Color.White));
        //protected texforge_definitions.Settings.Color endColor = new texforge_definitions.Settings.Color("End Color", new texforge_definitions.Types.Color(Color.White), new texforge_definitions.Types.Color(Color.Black), new texforge_definitions.Types.Color(Color.Black), new texforge_definitions.Types.Color(Color.White));

        protected Color startColor;
        protected Color endColor;

        public Color StartColor
        {
            get { return startColor; }
            set { startColor = value; }
        }

        public Color EndColor
        {
            get { return endColor; }
            set { endColor = value; }
        }

        public PerlinNoise(System.Drawing.Size size, System.Drawing.Imaging.PixelFormat pixelFormat)
            : base(size, pixelFormat)
        {
             
        }

        public override Atom Generate()
        {
            int octaveCount = 8;

            float[][] perlinNoise = GeneratePerlinNoise(size.Width, size.Height, octaveCount);
            Color[][] perlinImage = MapGradient(startColor, endColor, perlinNoise);

            return new Atom(size, pixelFormat, perlinImage);
            
        }

        public static Color[][] MapGradient(Color gradientStart, Color gradientEnd, float[][] perlinNoise)
        {
            int width = perlinNoise.Length;
            int height = perlinNoise[0].Length;

            Color[][] image = GetEmptyArray<Color>(width, height); //an array of colours

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    image[i][j] = GetColor(gradientStart, gradientEnd, perlinNoise[i][j]);
                }
            }

            return image;
        }


        public static T[][] GetEmptyArray<T>(int width, int height)
        {
            T[][] image = new T[width][];

            for (int i = 0; i < width; i++)
            {
                image[i] = new T[height];
            }

            return image;
        }

        static float[][] GenerateWhiteNoise(int width, int height)
        {
            Random random = new Random(0); //Seed to 0 for testing
            float[][] noise = GetEmptyArray<float>(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    noise[i][j] = (float)random.NextDouble() % 1;
                }
            }

            return noise;
        }

        public static float[][] GeneratePerlinNoise(float[][] baseNoise, int octaveCount)
        {
            int width = baseNoise.Length;
            int height = baseNoise[0].Length;

            float[][][] smoothNoise = new float[octaveCount][][]; //an array of 2D arrays containing

            float persistance = 0.7f;

            //generate smooth noise
            for (int i = 0; i < octaveCount; i++)
            {
                smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
            }

            float[][] perlinNoise = GetEmptyArray<float>(width, height); //an array of floats initialised to 0

            float amplitude = 1.0f;
            float totalAmplitude = 0.0f;
            
            //blend noise together
            for (int octave = octaveCount - 1; octave >= 0; octave--)
            {
                amplitude *= persistance;
                totalAmplitude += amplitude;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        perlinNoise[i][j] += smoothNoise[octave][i][j] * amplitude;
                    }
                }
            }

            //normalisation
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i][j] /= totalAmplitude;
                }
            }        

            return perlinNoise;
        }

        public static float[][] GeneratePerlinNoise(int width, int height, int octaveCount)
        {
            float[][] baseNoise = GenerateWhiteNoise(width, height);

            return GeneratePerlinNoise(baseNoise, octaveCount);
        }

        protected static Color[][] MapToGrey(float[][] greyValues)
        {
            int width = greyValues.Length;
            int height = greyValues[0].Length;

            Color[][] image = GetEmptyArray<Color>(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int grey = (int)(255 * greyValues[i][j]);
                    Color color = Color.FromArgb(255, grey, grey, grey);

                    image[i][j] = color;
                }
            }

            return image;
        }

        public static float[][] GenerateSmoothNoise(float[][] baseNoise, int octave)
        {
            int width = baseNoise.Length;
            int height = baseNoise[0].Length;

            float[][] smoothNoise = GetEmptyArray<float>(width, height);

            int samplePeriod = 1 << octave; // calculates 2 ^ k
            float sampleFrequency = 1.0f / samplePeriod;

            for (int i = 0; i < width; i++)
            {
                //calculate the horizontal sampling indices
                int sample_i0 = (i / samplePeriod) * samplePeriod;
                int sample_i1 = (sample_i0 + samplePeriod) % width; //wrap around
                float horizontal_blend = (i - sample_i0) * sampleFrequency;

                for (int j = 0; j < height; j++)
                {
                    //calculate the vertical sampling indices
                    int sample_j0 = (j / samplePeriod) * samplePeriod;
                    int sample_j1 = (sample_j0 + samplePeriod) % height; //wrap around
                    float vertical_blend = (j - sample_j0) * sampleFrequency;

                    //blend the top two corners
                    float top = Interpolate(baseNoise[sample_i0][sample_j0],
                        baseNoise[sample_i1][sample_j0], horizontal_blend);

                    //blend the bottom two corners
                    float bottom = Interpolate(baseNoise[sample_i0][sample_j1],
                        baseNoise[sample_i1][sample_j1], horizontal_blend);

                    //final blend
                    smoothNoise[i][j] = Interpolate(top, bottom, vertical_blend);
                }
            }

            return smoothNoise;
        }


        public static float Interpolate(float x0, float x1, float alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }

        public static Color Interpolate(Color col0, Color col1, float alpha)
        {
            float beta = 1 - alpha;
            return Color.FromArgb(
                255, //(int)(col0.A * alpha + col1.A * beta),
                (int)(col0.R * alpha + col1.R * beta),
                (int)(col0.G * alpha + col1.G * beta),
                (int)(col0.B * alpha + col1.B * beta));
        }

        public static Color GetColor(Color gradientStart, Color gradientEnd, float t)
        {
            float u = 1 - t;

            Color color = Color.FromArgb(
                255, //(int)(gradientStart.A * u + gradientEnd.A * t),
                (int)(gradientStart.R * u + gradientEnd.R * t),
                (int)(gradientStart.G * u + gradientEnd.G * t),
                (int)(gradientStart.B * u + gradientEnd.B * t));

            return color;
        }

    }
}
