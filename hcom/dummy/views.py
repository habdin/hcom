# vim: foldmethod=indent
#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django.http import JsonResponse
from django.shortcuts import get_object_or_404, redirect, render
from django.views import generic

from .forms import DummyForm
from .models import Dummy


# Function-based views for CRUD operations
#
# List of data that can be retrieved from a DB.
def list_view(request):
    dummies = Dummy.objects.all()
    return render(
        request, "dummy/dummy_list.html", {"title": "Dummy", "objects": dummies}
    )


# Read operation
def read_entry(request, pk):
    dummy = get_object_or_404(Dummy, pk=pk)
    # The above statement is equivalent to the following statements:
    # try:
    #     dummy = Dummy.objects.get(pk=pk)
    # except Dummy.DoesNotExist:
    #     raise Http404('Dummy does not exist.')
    return render(
        request,
        "dummy/dummy_detail.html",
        {
            "object": dummy,
        },
    )


# Create operation
def create_entry(request):
    if request.method == "POST":
        form = DummyForm(request.POST)
        if form.is_valid():
            form.save()
            # return redirect('/dummy/')
        result = True
        return JsonResponse(result, safe=False)
    else:
        form = DummyForm()
        return render(request, "dummy/dummy_form.html", {"form": form})


# Update operation
def update_entry(request, pk):
    dummy = get_object_or_404(Dummy, pk=pk)
    form = DummyForm(request.POST or None, instance=dummy)
    if form.is_valid():
        form.save()
        return redirect("/dummy/")
    return render(request, "dummy/dummy_form.html", {"form": form})


# Delete operation
def delete_entry(request, pk):
    dummy = get_object_or_404(Dummy, pk=pk)
    form = DummyForm(request.POST or None, instance=dummy)
    if form.is_valid():
        dummy.delete()
        return redirect("/dummy/")
    return render(
        request,
        "dummy/dummy_confirm_delete.html",
        {
            "form": form,
        },
    )


# Class-based views for CRUD operations
#
# Model List View
class DummyListView(generic.ListView):
    model = Dummy
    context_object_name = "objects"

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context["title"] = "Dummy List"
        return context


class DummySearchListView(generic.ListView):
    model = Dummy
    context_object_name = "objects"
    template_name = "dummy/card.html"

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context["title"] = "Dummy Search"
        return context

    def get_queryset(self, search=""):
        search = self.request.GET.get("search")
        if search == "":
            queryset = super().get_queryset()
        queryset = Dummy.objects.filter(name__icontains=search)
        return queryset


# Read operation
class DummyDetailView(generic.DetailView):
    model = Dummy


# Create operation
class DummyCreateView(generic.CreateView):
    model = Dummy
    form_class = DummyForm
    success_url = "/dummy/"


# Update operation
class DummyUpdateView(generic.UpdateView):
    model = Dummy
    form_class = DummyForm
    success_url = "/dummy/"


# Delete operation
class DummyDeleteView(generic.DeleteView):
    model = Dummy
    success_url = "/dummy"


def LoadData(request):
    """Load Model data as Json into a jquery datatable and provides the server-side searching and
    filtering for the table.
    """
    if request.method == "GET":
        draw = request.GET.get("draw")
    else:
        draw = request.POST.get("draw")
    dummies = Dummy.objects.all()
    count = len(dummies)
    ser_dummies = list(dummies.values("id", "name", "category"))
    response = {
        "draw": draw,
        "data": ser_dummies,
        "recordsTotal": count,
        "recordsFiltered": count,
    }
    return JsonResponse(response)
