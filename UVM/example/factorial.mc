int main()
{
	int result, n;

	read(&n);
	result = factorial(n);

	printf(result);
}

int factorial(int n)
{
	int j;
	if(n > 1)
		j = n -1;
		return n * factorial(j)
	else
		return 1;
}