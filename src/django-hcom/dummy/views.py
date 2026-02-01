# vim: foldmethod=indent
#!/usr/bin/env python3
# -*- coding: utf-8 -*-


from django.contrib.messages.views import SuccessMessageMixin
from django.conf import settings
from django.http import JsonResponse
from django.urls import reverse_lazy

# from django.shortcuts import get_object_or_404, redirect, render
from django.views import generic

from .forms import DummyForm
from .models import Dummy

# Function-based views for CRUD operations
#
# List of data that can be retrieved from a DB.
# def list_view(request):
#     dummies = Dummy.objects.all()
#     return render(
#         request, "dummy/dummy_list.html", {"title": "Dummy", "objects": dummies}
#     )


# Read operation
# def read_entry(request, pk):
#     dummy = get_object_or_404(Dummy, pk=pk)
#     # The above statement is equivalent to the following statements:
#     # try:
#     #     dummy = Dummy.objects.get(pk=pk)
#     # except Dummy.DoesNotExist:
#     #     raise Http404('Dummy does not exist.')
#     return render(
#         request,
#         "dummy/dummy_detail.html",
#         {
#             "object": dummy,
#         },
#     )


# Create operation
# def create_entry(request):
#     if request.method == "POST":
#         form = DummyForm(request.POST)
#         if form.is_valid():
#             form.save()
#             # return redirect('/dummy/')
#         result = True
#         return JsonResponse(result, safe=False)
#     else:
#         form = DummyForm()
#         return render(request, "dummy/dummy_form.html", {"form": form})


# Update operation
# def update_entry(request, pk):
#     dummy = get_object_or_404(Dummy, pk=pk)
#     form = DummyForm(request.POST or None, instance=dummy)
#     if form.is_valid():
#         form.save()
#         return redirect("/dummy/")
#     return render(request, "dummy/dummy_form.html", {"form": form})


# Delete operation
# def delete_entry(request, pk):
#     dummy = get_object_or_404(Dummy, pk=pk)
#     form = DummyForm(request.POST or None, instance=dummy)
#     if form.is_valid():
#         dummy.delete()
#         return redirect("/dummy/")
#     return render(
#         request,
#         "dummy/dummy_confirm_delete.html",
#         {
#             "form": form,
#         },
#     )


# Class-based views for CRUD operations
#
# Model List View
class DummyListView(generic.ListView):
    model = Dummy
    context_object_name = "objects"
    paginate_by = 10

    def get_context_data(self, **kwargs):
        view_as_table = self.request.GET.get("is_table", False)
        # The previous line of code is equivalent to the next commented lines of code.
        # Note that the variable name only has changed.
        # view_card = True
        # if self.request.GET.get("is_table"):
        # view_card = False
        context = super().get_context_data(**kwargs)
        context["is_table"] = bool(view_as_table)
        context["title"] = "Dummy List"
        context["debug"] = settings.DEBUG
        return context


# Modal Search View
class DummySearchListView(generic.ListView):
    model = Dummy
    context_object_name = "objects"
    template_name = "dummy/card.html"
    paginate_by = 10

    def get_queryset(self, search: str | None = ""):
        search = self.request.GET.get("search")
        if search:
            queryset = Dummy.objects.filter(name__icontains=search)
        else:
            queryset = super().get_queryset()
        return queryset


# Read operation
class DummyDetailView(generic.DetailView):
    model = Dummy


# Create operation
class DummyCreateView(SuccessMessageMixin, generic.CreateView):
    model = Dummy
    form_class = DummyForm
    success_url = reverse_lazy("dummy:dummy-list")
    success_message = "Entry was successfully created."


# Update operation
class DummyUpdateView(SuccessMessageMixin, generic.UpdateView):
    model = Dummy
    form_class = DummyForm
    success_url = reverse_lazy("dummy:dummy-list")
    success_message = "Entry was successfully updated."


# Delete operation
class DummyDeleteView(SuccessMessageMixin, generic.DeleteView):
    model = Dummy
    success_url = reverse_lazy("dummy:dummy-list")
    success_message = "Entry was successfully updated."


def LoadData(request):
    """Load Model data as Json into a jquery DataTable and provide the server-side searching, ordering and
    filtering for the table.
    """
    # Extract request parameters:
    draw = (
        request.GET.get("draw") if request.method == "GET" else request.POST.get("draw")
    )
    start = int(request.GET.get("start", 0))
    length = int(request.GET.get("length", 10))
    search_str = request.GET.get("search[value]", "")
    ordering_columns_index = int(request.GET.get("order[0][column]", 0))
    ordering_direction = str(request.GET.get("order[0][dir]", "asc"))

    # Filter data based on search string
    if search_str:
        dummies = Dummy.objects.filter(name__icontains=search_str)
    else:
        dummies = Dummy.objects.all()

    # Map the ordering column to model field names
    ordering_field = ["id", "name", "category"][ordering_columns_index]

    # Determine the sorting order direction
    if ordering_direction == "desc":
        ordering_field = f"-{ordering_field}"

    # Get the total records before filtering
    total_Count = Dummy.objects.count()

    # Get the count after filtering
    recordsFiltered = dummies.count()

    # Paginate the queryset
    dummies = dummies.order_by(ordering_field)[start : start + length]

    # Serialize the data
    ser_dummies = list(dummies.values("id", "name", "category"))

    response = {
        "draw": draw,
        "data": ser_dummies,
        "recordsTotal": total_Count,
        "recordsFiltered": recordsFiltered,
    }
    return JsonResponse(response)
