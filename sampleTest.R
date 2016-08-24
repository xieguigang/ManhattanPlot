# 生成测试数据集

size <- 5000

Chr = sample(1:24,size,replace=T)
Position = sample(1:247249719,size)

sample1 = runif(size)
sample2 = runif(size)
sample3 = runif(size)
sample4 = runif(size)
sample5 = runif(size)
sample6 = runif(size)

df <- data.frame(Chr, Position, sample1, sample2, sample3, sample4, sample5, sample6)

write.csv(df, "./manhattan_plot_test.csv")