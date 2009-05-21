//Calculating Prime Factors in C# - http://www.blackwasp.co.uk/PrimeFactors.aspx
//
//The prime factors of a number are all of the prime numbers that can be divided into
//the number without a remainder. In this article, we will create a method that uses 
//direct search factorisation to generate the list of prime factors for a given value.
using System.Collections;
using System.Collections.Generic;
public class Eratosthenes : IEnumerable
{
    private static List<int> _primes = new List<int>();
    private int _lastChecked;


    public Eratosthenes()
    {
        _primes.Add(2);
        _lastChecked = 2;
    }
	private bool IsPrime(int checkValue)
	{
	    bool isPrime = true;
	
	    foreach (int prime in _primes)
	    {
	        if ((checkValue % prime) == 0 && prime <= System.Math.Sqrt(checkValue))
	        {
	            isPrime = false;
	            break;
	        }
	    }
	
	    return isPrime;
	}

	public IEnumerator<int> GetEnumerator()
	{
	    foreach (int prime in _primes)
	        yield return prime;
	
	    while (_lastChecked < int.MaxValue)
	    {
	        _lastChecked++;
	
	        if (IsPrime(_lastChecked))
	        {
	            _primes.Add(_lastChecked);
	            yield return _lastChecked;
	        }
	    }
	}
	
	IEnumerator IEnumerable.GetEnumerator()
	{
	    return GetEnumerator();
	}
	
	private static IEnumerable<int> GetPrimeFactors(int value)
	{
		Eratosthenes eratosthenes = new Eratosthenes();
	    List<int> factors = new List<int>();
	
	    foreach (int prime in eratosthenes)
	    {
	        while (value % prime == 0)
	        {
	            value /= prime;
	            factors.Add(prime);
	        }
	
	        if (value == 1)
	            break;
		}
	    return factors;
	}
}