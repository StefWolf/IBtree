using System;
using System.Collections.Generic;
using System.Linq;

/** Fila de prioridade genérica. */
public class PQueue<T>
{
    private readonly Func<T, T, T> _priorFunc;
    private readonly List<T> _queue;

    /** Cria uma fila cuja prioridade é dada por uma função que recebe dois elementos e retorna o de maior prioridade. */
    public PQueue(Func<T, T, T> priorFunc)
    {
        _priorFunc = priorFunc;
        _queue = new List<T>();
    }

    /** Insere um elemento na fila de prioridade. */
    public void Push(T data)
    {
        _queue.Add(data);
    }

    /** Remove e retorna o elemento de maior prioridade (de acordo com a função passada no construtor). */
    public T Pop()
    {
        if (_queue.Count > 0)
        {
            var elm = _queue.Aggregate(_priorFunc);
            _queue.Remove(elm);
            return elm;
        }
        return default;
    }

    /** Verifica se a fila está vazia. */
    public bool IsEmpty()
    {
        return !_queue.Any();
    }

    /**
     * Encontra um elemento na fila (não remove).
     * Se uma função for passada, a comparação será realizada a partir da função.
     */
    public T Find(object data)
    {
        if (data is Func<T, bool> comp)
        {
            return _queue.Find(new Predicate<T>(comp));
        }
        return _queue.Find(elm => Equals(elm, data));
    }
}
