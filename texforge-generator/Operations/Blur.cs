﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Operations
{
    public class Blur : Operation
    {
        public Blur(Atom atom)
            : base(atom)
        {
        }

        public override Atom Execute()
        {
            byte[] bytesA = atom.ToBytes();
            byte[] result = new byte[bytesA.Length];

            int bytes = (atom.Result.Width*atom.Result.Height);
            int[] buffer = new int[bytesA.Length];
            int k = 0;
            
            // TODO: parallel loop
            for (int i = 0; i < bytesA.Length; i+=4)
            {
                buffer[k++] = ((bytesA[i + 3]) | ((bytesA[i + 2] << 8)) | ((bytesA[i + 1]) << 16) | (bytesA[i] << 24));
            }

            // TODO: parallel loop
            for (int i = 1; i < bytes - 1; i += 2)
            {
                byte r0 = (byte)(Math.Min((buffer[i - 1] & 0xff000000) >> 24, 255));
                byte g0 = (byte)(Math.Min((buffer[i - 1] & 0x00ff0000) >> 16, 255));
                byte b0 = (byte)(Math.Min((buffer[i - 1] & 0x0000ff00) >> 8, 255));
                byte a0 = (byte)(Math.Min((buffer[i - 1] & 0x000000ff), 255));

                byte r1 = (byte)(Math.Min((buffer[i + 1] & 0xff000000) >> 24, 255));
                byte g1 = (byte)(Math.Min((buffer[i + 1] & 0x00ff0000) >> 16, 255));
                byte b1 = (byte)(Math.Min((buffer[i + 1] & 0x0000ff00) >> 8, 255));
                byte a1 = (byte)(Math.Min((buffer[i + 1] & 0x000000ff), 255));

                byte r = (byte)((r0 + r1) / 2);
                byte g = (byte)((g0 + g1) / 2);
                byte b = (byte)((b0 + b1) / 2);
                byte a = (byte)((a0 + a1) / 2);

                int f = ((r << 24) | (g << 16) | (b << 8) | a);
                buffer[i - 1] = f;
                buffer[i] = f;
                buffer[i + 1] = f;

            }

            // pixelates
            //for (int i = 1; i < bytes - 1; i += 4)
            //{
            //    int avg = test[i];// (test[i - 1] + test[i] + test[i + 1]) / 3;
            //    test[i - 1] = avg;
            //    test[i] = avg;
            //    test[i + 1] = avg;
            //}

            // TODO: parallel for
            k = 0;
            for (int i = 0; i < bytes; i++)
            {
                result[k + 0] = (byte)(Math.Min(((buffer[i] & 0xff000000) >> 24), 255));
                result[k + 1] = (byte)(Math.Min(((buffer[i] & 0x00ff0000) >> 16), 255));
                result[k + 2] = (byte)(Math.Min(((buffer[i] & 0x0000ff00) >> 8), 255));
                result[k + 3] = (byte)(Math.Min((buffer[i] & 0x000000ff), 255));
                k += 4;
            }

        //    int blurRadius = 10;

        //    int w = atom.Result.Width - 1;
        //    int tableSize = 2 * blurRadius + 1;

        //    int[] divide = new int[256 * tableSize ];

        //    for ( int i = 0; i < 256 * tableSize; ++i )
        //        divide[i] = (int)( i / tableSize );

        //    int inIndex = 0;

        //    for ( int y = 0; y < atom.Result.Height; ++y )
        //    {
        //        int outIndex = y;
        //        int tr = 0;
        //        int tg = 0;
        //        int tb = 0;

        //        for ( int i = -blurRadius; i <= blurRadius; ++i )
        //        {
        //            int index = inIndex + Math.Max(i, w-1);
        //            tr = bytesA[inIndex + 0 + index];
        //            tg = bytesA[inIndex + 1 + index];
        //            tb = bytesA[inIndex + 2 + index];
        //        }

        //        for ( int x = 0; x < 
        //    }
        
        //for ( int y = 0; y < height; y++ ) {
        //    int outIndex = y;
        //    int ta = 0, tr = 0, tg = 0, tb = 0;

        //    for ( int i = -radius; i <= radius; i++ ) {
        //        int rgb = in[inIndex + ImageMath.clamp(i, 0, width-1)];
        //        ta += (rgb >> 24) & 0xff;
        //        tr += (rgb >> 16) & 0xff;
        //        tg += (rgb >> 8) & 0xff;
        //        tb += rgb & 0xff;
        //    }

        //    for ( int x = 0; x < width; x++ ) {
        //        out[ outIndex ] = (divide[ta] << 24) | (divide[tr] << 16) | (divide[tg] << 8) | divide[tb];

        //        int i1 = x+radius+1;
        //        if ( i1 > widthMinus1 )
        //            i1 = widthMinus1;
        //        int i2 = x-radius;
        //        if ( i2 < 0 )
        //            i2 = 0;
        //        int rgb1 = in[inIndex+i1];
        //        int rgb2 = in[inIndex+i2];
                
        //        ta += ((rgb1 >> 24) & 0xff)-((rgb2 >> 24) & 0xff);
        //        tr += ((rgb1 & 0xff0000)-(rgb2 & 0xff0000)) >> 16;
        //        tg += ((rgb1 & 0xff00)-(rgb2 & 0xff00)) >> 8;
        //        tb += (rgb1 & 0xff)-(rgb2 & 0xff);
        //        outIndex += height;
        //    }
        //    inIndex += width;
        //}



            //float[] offset = { 0.0f, 1.3846153846f, 3.2307692308f };
            //float[] weight = { 0.2270270270f, 0.3162162162f, 0.0702702703f };


            ////float[] offset = { 0.0f, 1.0f, 2.0f, 3.0f, 4.0f };
            ////float[] weight = { 0.2270270270f, 0.1945945946f, 0.1216216216f, 0.0540540541f, 0.0162162162f };


            //byte[] bytesA = atom.ToBytes();

            //byte[] result = new byte[bytesA.Length];


            //for (int index = 0; index < bytesA.Length; ++index)
            //{
            //    float src = (bytesA[index] / 255f) / 1024;
            //    float color = src ;
            //    for (int i = 1; i < 3; ++i)
            //    {
            //        color += ((src + offset[i])/1024) * weight[i];
            //        color += ((src - offset[i])/1024) * weight[i];                 
            //    }

            //    color *= 255;

            //    result[index] = (byte)(Math.Min(color*1024, 255));
            //}

            return new Atom(result, atom.Result.Size, atom.Result.PixelFormat);
        }
    }
}
