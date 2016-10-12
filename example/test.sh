#!/bin/bash

rm ./manhattan_plot_test_chr.png
rm ./manhattan_plot_test_interval.png
rm ./manhattan_plot_test_SampleName.png

../ManhattanPlot.exe /Draw /in "./manhattan_plot_test.csv" /pt.size 10 /sampleColors "./SampleColors.csv" /colorpattern SampleName
../ManhattanPlot.exe /Draw /in "./manhattan_plot_test.csv" /pt.size 10 /colorpattern chr
../ManhattanPlot.exe /Draw /in "./manhattan_plot_test.csv" /pt.size 10 /colorpattern interval