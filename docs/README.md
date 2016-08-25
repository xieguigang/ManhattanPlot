# ManhattanPlot [version 1.0.0.0]
> 
> ### ManhattanPlot
> Manhattan plot in VisualBasic
> 
> ##### [Manhattan plot](https://en.wikipedia.org/wiki/Manhattan_plot)
> 
> A Manhattan plot is a type of scatter plot, usually used to display data with a large number of data-points - many of non-zero amplitude, and with a distribution of higher-magnitude values, for instance in genome-wide association studies (GWAS).[1] In GWAS Manhattan plots, genomic coordinates are displayed along the X-axis, with the negative logarithm of the association P-value for each single nucleotide polymorphism (SNP) displayed on the Y-axis, meaning that each dot on the Manhattan plot signifies a SNP. Because the strongest associations have the smallest P-values (e.g., 10−15), their negative logarithms will be the greatest (e.g., 15).
> 
> It gains its name from the similarity of such a plot to the Manhattan skyline: a profile of skyscrapers towering above the lower level "buildings" which vary around a lower height.
> 
> ##### References
> + Gibson, Greg (2010). "Hints Of hidden heritability In GWAS". Nature Genetics. 42 (7): 558–560. doi:10.1038/ng0710-558. PMID 20581876.

<!--more-->

**ManhattanPlot**
__
Copyright © GPL3 2016

**Module AssemblyName**: file:///G:/ManhattanPlot/ManhattanPlot/bin/Debug/ManhattanPlot.exe
**Root namespace**: ``ManhattanPlot.Program``


All of the command that available in this program has been list below:

|Function API|Info|
|------------|----|
|[/Draw](#/Draw)|Invoke the Manhattan plots for the SNP sites.|


## CLI API list
--------------------------
<h3 id="/Draw"> 1. /Draw</h3>

Invoke the Manhattan plots for the SNP sites.
**Prototype**: ``ManhattanPlot.Program::Int32 Draw(Microsoft.VisualBasic.CommandLine.CommandLine)``

###### Usage
```bash
ManhattanPlot /Draw /in <data.csv> [/out <out.png> /sampleColors <sample_colors.csv> /width 3000 /height 1440 /pt.size 25 /debug.label /equidistant /relative /ylog <ln/log/raw, default:=ln> /colorPattern <chr/sampleName/interval, default:=chr>]
```
###### Example
```bash
ManhattanPlot /Draw /in ./manhattan_plot_test.csv /out ./manhattan_plot_test.png
```



#### Parameters information:
##### /in

###### Example
```bash

```
##### [/sampleColors]
Color expression supports both .NET known color name and rgb expression.

+ .NET known color names: https://github.com/xieguigang/VisualBasic_AppFramework/blob/master/VB.NET_Colors.html
+ rgb expressions: ``rgb(r,g,b)`` or ``rgb(a,r,g,b)``, parameters ``a,r,g,b`` each value should less than 256, that is value ranges from 0 to 255

###### Example
```bash

```
##### [/ylog]

+ ``ln``, for ``-ln(p-value)``, log value on base ``e``
+ ``log``, for ``-log(p-value, 10)``, log value on base 10
+ ``raw``, for raw value, no transformation.

###### Example
```bash

```
##### Accepted Types
###### /in
**Decalre**:  _ManhattanPlot.SNP[]_
Example: 
```json
[
    {
        "Extension": {
            "DynamicHash": {
                "Properties": [
                    
                ],
                "source": [
                    
                ]
            }
        },
        "Chr": "System.String",
        "Gene": "System.String",
        "Position": 0,
        "SNP": "System.String",
        "pvalues": [
            
        ]
    }
]
```

###### /sampleColors
**Decalre**:  _ManhattanPlot.SampleColor[]_
Example: 
```json
[
    {
        "Color": "System.String",
        "SampleName": "System.String"
    }
]
```

###### /ylog
**Decalre**:  _System.String_
Example: 
```json
"System.String"
```

